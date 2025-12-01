using FluentValidation;
using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.Settings.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            IReadOnlyList<string> validationErrors = [.. failures.Select(error => error.ErrorMessage)];
            return CreateFailureResponse<TResponse>(validationErrors);
        }

        return await next(cancellationToken);
    }

    private static TResp CreateFailureResponse<TResp>(IReadOnlyList<string> errors)
    {
        var responseType = typeof(TResp);
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
        {
            var failureMethod = responseType.GetMethod("Failure", [typeof(string[])]);
            if (failureMethod != null)
            {
                var result = failureMethod.Invoke(null, [errors.ToArray()]);
                return (TResp)result!;
            }
        }
        throw new InvalidOperationException("No se pudo crear una respuesta de error.");
    }
}
