using API.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Utilities;

public sealed class ExceptionUtility(ILogger<ExceptionUtility> logger, IServiceProvider services)
{
    /// <summary>
    /// Handles public/private error results for API controller routes.
    /// </summary>
    /// <param name="callback">The callback to run.</param>
    /// <typeparam name="TException">The exception that will be publicly returned to the user.</typeparam>
    /// <returns>The result of the request.</returns>
    public async Task<IActionResult> HandleHttpException<TException, TRequest>(
        TRequest request,
        Func<Task<IActionResult>> callback)
        where TException : BaseException
    {
        try
        {
            var validator = services.GetRequiredService<IValidator<TRequest>>();
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new ObjectResult(
                        new ValidationErrorResult([.. validation.Errors.Select(error => error.ErrorMessage)])
                    )
                    { StatusCode = 400 };
            }

            return await callback();
        }
        catch (Exception exception)
        {
            if (exception is TException baseException)
            {
                return new ObjectResult(baseException.Message) { StatusCode = baseException.Code };
            }

            logger.LogError(exception, "Exception occurred: {EXCEPTION_MESSAGE}", exception.Message);
            return new ObjectResult("Internal server error.") { StatusCode = 500 };
        }
    }

    /// <summary>
    /// Handles public/private error results for API controller routes.
    /// </summary>
    /// <param name="callback">The callback to run.</param>
    /// <typeparam name="TException">The exception that will be publicly returned to the user.</typeparam>
    /// <returns>The result of the request.</returns>
    public async Task<IActionResult> HandleHttpException<TException>(
        Func<Task<IActionResult>> callback)
        where TException : BaseException
    {
        try
        {
            return await callback();
        }
        catch (Exception exception)
        {
            if (exception is TException baseException)
            {
                return new ObjectResult(baseException.Message) { StatusCode = baseException.Code };
            }

            logger.LogError(exception, "Exception occurred: {EXCEPTION_MESSAGE}", exception.Message);
            return new ObjectResult("Internal server error.") { StatusCode = 500 };
        }
    }
}

public record ValidationErrorResult(List<string> Errors);