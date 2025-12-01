using VotingSurvey.Presentation.Middleware;

namespace VotingSurvey.Presentation.Settings
{
    public static class SettingPresentation
    {
        public static IServiceCollection Presentation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddTransient<ApiMiddleware>();
            serviceCollection.AddSignalR();

            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return serviceCollection;
        }
    }
}
