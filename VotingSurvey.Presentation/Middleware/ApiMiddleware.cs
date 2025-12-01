using System.Text.Json;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Presentation.Middleware
{
    public class ApiMiddleware : IMiddleware
    {
        private const string RoleHeader = "X-Role";
        private const string UserIdHeader = "X-User-Id";
        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            "ADMIN","RESIDENT"
        };

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Extract role
                if (!context.Request.Headers.TryGetValue(RoleHeader, out var roleValues))
                {
                    await WriteFailureAsync(context, 400, "Missing role header");
                    return;
                }
                var role = roleValues.ToString().Trim();
                if (!AllowedRoles.Contains(role))
                {
                    await WriteFailureAsync(context, 403, "Invalid or unauthorized role");
                    return;
                }

                // Extract user id (optional for ADMIN create flows but enforce presence for vote operations)
                Guid? userId = null;
                if (context.Request.Headers.TryGetValue(UserIdHeader, out var userValues))
                {
                    if (Guid.TryParse(userValues.ToString(), out var parsed))
                        userId = parsed;
                    else
                    {
                        await WriteFailureAsync(context, 400, "Invalid user id format");
                        return;
                    }
                }

                // Store in HttpContext for downstream handlers
                context.Items["Role"] = role.ToUpperInvariant();
                if (userId is not null) context.Items["UserId"] = userId.Value;

                await next(context);
            }
            catch (Exception ex)
            {
                await WriteFailureAsync(context, 500, ex.Message);
            }
        }

        private static async Task WriteFailureAsync(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted) return;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Failure(new List<string> { message });
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

    public static class ApiMiddlewareExtensions
    {
        public static IServiceCollection AddApiMiddleware(this IServiceCollection services)
        {
            services.AddTransient<ApiMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiMiddleware>();
        }
    }
}
