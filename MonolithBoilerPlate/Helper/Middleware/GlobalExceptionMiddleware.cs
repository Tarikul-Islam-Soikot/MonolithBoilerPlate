using MonolithBoilerPlate.Common.Models;
using MonolithBoilerPlate.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonolithBoilerPlate.Api.Helper.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new GlobalError();

            if (exception is BadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                errorResponse.ErrorName = "Bad Request";
                errorResponse.Detail = exception.Message;
            }
            else if (exception is UnAuthorizedException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                errorResponse.ErrorName = "Un-Authorized";
                errorResponse.Detail = exception.Message;
            }
            else if (exception is DuplicateException)
            {
                context.Response.StatusCode = StatusCodes.Status412PreconditionFailed;
                errorResponse.ErrorName = "Duplicate Entry";
                errorResponse.Detail = exception.Message;
            }
            else if (exception is NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                errorResponse.ErrorName = "Not Found";
                errorResponse.Detail = exception.Message;
            }
            else if (exception is DBConcurrencyException)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                errorResponse.ErrorName = "Concurrent Request Error";
                errorResponse.Detail = "The data your are trying to update is already updated. Please try again after re-fetch.";
            }
            else if (exception is DbUpdateException)
            {
                SqlException? sqlException = exception.InnerException as SqlException;
                if (sqlException?.Number == 547)
                {
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    errorResponse.ErrorName = "Database dependency error";
                    errorResponse.Detail = "Please resolve dependencies with " + Regex.Replace(sqlException.Message.Split('_')[1], "(\\B[A-Z])", " $1");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.ErrorName = "Internal Server Error";
                    errorResponse.Detail = exception.Message ?? exception.InnerException?.Message ?? "An unknown error occurred.";
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse.ErrorName = "Internal Server Error";
                errorResponse.Detail = exception.Message ?? exception.InnerException?.Message ?? "An unknown error occurred.";
            }

            var jsonOptions = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            context.Response.ContentType = "application/json";
            var jsonResponse = JsonConvert.SerializeObject(errorResponse, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);

            errorResponse.StackTrace = exception.ToString();
            var errorJson = JsonConvert.SerializeObject(errorResponse, jsonOptions);
            _logger.LogCritical(errorJson);
        }
    }

}
