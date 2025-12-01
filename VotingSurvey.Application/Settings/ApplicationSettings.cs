using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VotingSurvey.Application.Settings.Behaviors;

namespace VotingSurvey.Application.Settings
{
    public static class ApplicationSettings
    {
        public static IServiceCollection Application(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ApplicationSettings).Assembly);
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(ApplicationSettings).Assembly);

            return services;
        }
    }
}
