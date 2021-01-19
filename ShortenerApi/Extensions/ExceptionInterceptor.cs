using Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ShortenerApi
{
    public static class ExceptionInterceptor
    {
        public static IServiceCollection AddExceptionInterceptor(this IServiceCollection services)
        {
            services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<ApiBehaviorOptions>>();
                var problemDetailsFactory = new CustomProblemDetailsFactory(options);
                problemDetailsFactory.Map<BusinessRuleValidationException>((ex, context) =>
                    new ProblemDetails()
                    {
                        Title = ex.Message,
                        Detail = ex.Details,
                        Status = StatusCodes.Status409Conflict,
                    });


                return problemDetailsFactory;
            });

            return services;
        }
    }
}
