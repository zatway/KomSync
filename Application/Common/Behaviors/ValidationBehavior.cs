using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

/// <summary>
/// Перехватывает каждую команду MediatR и запускает для неё валидаторы FluentValidation.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            // Запускаем все валидаторы одновременно
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Собираем все ошибки в один список
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }

        // Если ошибок нет или валидаторов не нашлось — идем к следующему шагу (в Handler)
        return await next();
    }
}