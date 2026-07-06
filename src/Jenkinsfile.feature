_pipeline{
    agent any

    when{
        not {branch 'main'}
    }

    environment{
        DOCKERHUB_CREDENTIALS = credentials('dockerhub-credentials')
        DOCKERHUB_USER = "${DOCKERHUB_CREDENTIALS_USR}"
        BRANCH_TAG = "${env.BRANCH_NAME.replaceAll('[/.]', '-')}-${env.BUILD_NUMBER}"
        
    }

    stages{
        stage('Checkout'){
            steps{
                checkout scm
                echo "Branch: ${env.BRANCH_NAME} | Image tag will be: ${BRANCH_TAG}"
            }
        }

        stage('Install Trivy'){
            steps{
                sh '''
                  if ! command -v trivy &> /dev/null; then
                      echo "Installing Trivy....."
                      curl -sfL https://raw.githubusercontent.com/aquasecurity/trivy/main/contrib/install.sh | sh -s -- -b /usr/local/bin

                      else
                        echo "Trivy already installed: $(trivy --version)"
                      fi

                    '''
            }
        }

        stage('Build, Scan $ push'){
            steps{
                def services = [
                    [name: 'catalog-api', service: 'src/Services/Catalog/CatalogAPI/Dockerfile'],
                                   [name: 'basket-api', dockerfile: 'src/Services/Basket/Basket.API/Dockerfile'],
                                   [name: 'discount-grpc', dockerfile: 'src/Services/Discount/Discount.Grpc/Dockerfile'],
                                   [name: 'ordering-api', dockerfile: 'src/Services/Ordering/Ordering.API/Dockerfile'],
                                   [name: 'yarpapigateway', dockerfile: 'src/ApiGateways/YarpApiGateway/Dockerfile'],
                                   [name: 'shopping-web', dockerfile: 'src/WebApps/Shopping-Web/Dockerfile']
                ]

                sh "echo ${DOCKERHUB_CREDENTIALS_PSW} | docker login -u ${DOCKERHUB_CREDENTIALS_USR} --password-stdin"

                for(svc in Services){
                    def fullTag = "${DOCKERHUB_USER}/${svc.name}:${BRANCH_TAG}"

                    echo "=== Building ${svc.name} ===="
                    sh "docker build -f ${svc.dockerfile} -t ${fullTag} ."

                    echo "==== Scanning ${svc.name} with Trivy ===="

                    sh """
                      trivy image \
                        --exit-code 1 \
                        --severity MEDIUM \
                        --no-progress \
                        --format table \
                        ${fullTag}
                        """

                    echo "=== Pushing ${svc.name} ==="
                    sh "docker push ${fullTag}"

                    sh "docker image prune -f"
                    
                }
            }
        }
    }

    post{
        always {
            sh 'docker logout || true'
        }
        success{
            echo "feature branch ${env.BRANCH_NAME} passed Trivy scan. Image tagged: ${BRANCH_TAG}. Not deployed (featured branch)."

        }
        failure{
            echo "Build FAILED for branch ${env.BRANCH_NAME}. check Trivy output above for HIGH/CRITICAL vulnerabilities or build errors."
        }

    }
}

