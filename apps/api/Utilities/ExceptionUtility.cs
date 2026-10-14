using API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Utilities;

public static class ExceptionUtility
{
    /// <summary>
    /// Handles public/private error results for API controller routes.
    /// </summary>
    /// <param name="callback">The callback to run.</param>
    /// <typeparam name="TException">The exception that will be publicly returned to the user.</typeparam>
    /// <returns>The result of the request.</returns>
    public static async Task<IActionResult> HandleHttpException<TException>(Func<Task<IActionResult>> callback)
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

            return new ObjectResult("Internal server error.") { StatusCode = 500 };
        }
    }
}