pipeline{
    agent any


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
                script{
                    def services = [
                                   [name: 'catalogapi', dockerfile: 'src/Services/Catalog/CatalogAPI/Dockerfile'],
                                   [name: 'basketapi', dockerfile: 'src/Services/Basket/Basket.API/Dockerfile'],
                                   [name: 'discountgrpc', dockerfile: 'src/Services/Discount/Discount.Grpc/Dockerfile'],
                                   [name: 'orderingapi', dockerfile: 'src/Services/Ordering/Ordering.API/Dockerfile'],
                                   [name: 'yarpapigateway', dockerfile: 'src/ApiGateways/YarpApiGateway/Dockerfile'],
                                   [name: 'shopping-webapp', dockerfile: 'src/WebApps/Shopping-Web/Dockerfile']
                ]

                    sh "echo ${DOCKERHUB_CREDENTIALS_PSW} | docker login -u ${DOCKERHUB_CREDENTIALS_USR} --password-stdin"

                    for(svc in services){

                        def fullTag = "${DOCKERHUB_USER}/${svc.name}:${BRANCH_TAG}"

                        echo "=== Building ${svc.name} ===="
                        sh "docker build -f ${svc.dockerfile} -t ${fullTag} src"

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
    }

    post{
        always {
            node{
                sh 'docker logout || true'
            }
            
        }
        success{
            echo "feature branch ${env.BRANCH_NAME} passed Trivy scan. Image tagged: ${BRANCH_TAG}. Not deployed (featured branch)."

        }
        failure{
            echo "Build FAILED for branch ${env.BRANCH_NAME}. check Trivy output above for HIGH/CRITICAL vulnerabilities or build errors."
        }

    }
}

