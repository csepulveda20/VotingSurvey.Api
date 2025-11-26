using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VotingSurvey.Infrastructure.Settings
{
    public static class InfrastructureSettings
    {
        public static IServiceCollection Infrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {



            return serviceCollection;
        }
    }
}
