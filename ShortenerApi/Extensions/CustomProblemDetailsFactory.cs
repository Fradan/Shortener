using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Application
{
    using MapDict = Dictionary<Type, Func<Exception, HttpContext, ProblemDetails>>;

    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;
        private readonly MapDict _mapDict = new MapDict();

        public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        } 



        public void Map<T>(Func<T, HttpContext, ProblemDetails> func) where T : Exception
        {
            _mapDict.Add(typeof(T), (ex, context) => func((T)ex, context));
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            statusCode ??= StatusCodes.Status500InternalServerError;

            ProblemDetails pd = null;
            if (httpContext.Features.Get<IExceptionHandlerFeature>()?.Error is { } error)
            {
                pd = GetProblemDetailsOrDefault(error, httpContext);
            }

            if (pd == null)
            {
                pd = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Type = type,
                    Detail = detail,
                    Instance = instance
                };
            }

            ApplyProblemDetailsDefaults(httpContext, pd, statusCode.Value);

            return pd;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= StatusCodes.Status400BadRequest;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            if (title != null)
            {
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int status)
        {
            problemDetails.Status ??= status;

            if (_options.ClientErrorMapping.TryGetValue(status, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;

                var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
                if (traceId != null)
                {
                    problemDetails.Extensions["traceId"] = traceId;
                }
            }
        }

        private ProblemDetails GetProblemDetailsOrDefault(Exception ex, HttpContext context)
        {
            var mapMethod = _mapDict.GetValueOrDefault(ex.GetType());
            return mapMethod?.Invoke(ex, context);
        }
    }
}
