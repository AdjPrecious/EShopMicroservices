﻿using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.behaviours
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                
            var failure = validationResult.Where( r => r.Errors.Any()).SelectMany(r => r.Errors)
                .ToList();
            if(failure.Any())
                throw new ValidationException(failure);

            return await next();
        }
    }
}
