using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        private readonly bool includeDetails = false;

        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.next = next;
            
            if (environment.IsDevelopment())
            {
                this.includeDetails = true;
            }
            else
            {
                string includeStackTraceConfig = configuration.GetSection("Logging")
                                                              .GetSection("ProblemDetails")
                                                              .GetSection("ForceIncludeStackTrace").Value;
                if (bool.TryParse(includeStackTraceConfig, out bool includeStackTrace))
                {
                    this.includeDetails = includeStackTrace;
                }
            }
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateConcurrencyException dbucException)
                {
                    httpContext.Response.ContentType = "application/problem+json";
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    await httpContext.Response.WriteAsync(
                        JsonConvert.SerializeObject(CreateProblemDetails(dbucException, StatusCodes.Status409Conflict, "Conflict")));
                }
                else
                {
                    httpContext.Response.ContentType = "application/problem+json";
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await httpContext.Response.WriteAsync(
                        JsonConvert.SerializeObject(CreateProblemDetails(ex, StatusCodes.Status500InternalServerError, "Internal Server Error")));
                }
            }
        }

        private ProblemDetails CreateProblemDetails(Exception ex, int status, string? title = null)
        {
            var problemDetails = new ProblemDetails
            {
                Title = includeDetails ? ex.GetType().Name : title,
                Detail = includeDetails ? ex.Message : string.Empty,
                Status = status,
                Type = $"https://http.cat/{status}"
            };

            if (includeDetails)
            {
                problemDetails.Extensions.Add(nameof(ex.StackTrace), ex.StackTrace);

                int count = 1;
                Exception innerException = ex.InnerException;
                while (innerException is not null)
                {
                    problemDetails.Extensions.Add($"{innerException.GetType().Name}-{count++}", innerException.Message);
                    innerException = innerException.InnerException;
                }
            }

            return problemDetails;
        }
    }
}
