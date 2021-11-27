using CleanArchitecture.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace CleanArchitecture.Web.Api.Filter
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult(this ControllerBase controller, IResult result)
        {
            ObjectResult ProblemDetails(string title, int status)
                => controller.Problem(result.ErrorMessage, null, status, title, GetType(status));

            ActionResult ValidationProblemDetails(string title, int status)
            {
                ModelStateDictionary errors = new ModelStateDictionary();
                foreach (var error in result.ValidationErrors)
                {
                    errors.AddModelError(error.Identifier, error.Description);
                }

                return controller.ValidationProblem(result.ErrorMessage, null, status, title, GetType(status), errors);
            }

            return result.Status switch
            {
                ResultStatus.Success => controller.Ok(result.GetValue()),
                ResultStatus.SuccessNoResult => controller.NoContent(),
                ResultStatus.BadRequest => ValidationProblemDetails("Bad Request", StatusCodes.Status400BadRequest),
                ResultStatus.Unauthorized => ProblemDetails("Unauthorized", StatusCodes.Status401Unauthorized),
                ResultStatus.Forbidden => ProblemDetails("Forbidden", StatusCodes.Status403Forbidden),
                ResultStatus.NotFound => ProblemDetails("Not Found", StatusCodes.Status404NotFound),
                _ => throw new ArgumentOutOfRangeException(nameof(result.Status), $"Value {result.Status} cannot be mapped to a http status code.")
            };
        }

        private static string GetType(int status) => $"https://http.cat/{status}";
    }
}
