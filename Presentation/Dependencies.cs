using Application.DTOs;

namespace Presentation;

public static class Dependencies
{
    public static void AddDependentServices(this IServiceCollection services, ConfigurationManager? configuration)
    {

        services.AddGlobalErrorHandling();

    }

    private static void AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Title = "Bad Request";
                context.ProblemDetails.Status = StatusCodes.Status400BadRequest;
                context.ProblemDetails.Detail = "There's error during processing request";
                context.ProblemDetails.Extensions["exception"] = context.Exception?.Message;
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });
    }
}

