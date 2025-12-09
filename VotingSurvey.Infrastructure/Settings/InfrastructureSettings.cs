using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VotingSurvey.Application.Repositories;
using VotingSurvey.Application.Services;
using VotingSurvey.Infrastructure.Persistence.Context;
using VotingSurvey.Infrastructure.Persistence.Repositories;
using VotingSurvey.Infrastructure.Persistence.Services;

namespace VotingSurvey.Infrastructure.Settings
{
    public static class InfrastructureSettings
    {
        public static IServiceCollection Infrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DataBaseConnection"))
            );

            serviceCollection.AddScoped<ICommunity, CommunityRepository>();
            serviceCollection.AddScoped<IVote, VoteRepository>();
            serviceCollection.AddScoped<IUser, UserRepository>();
            serviceCollection.AddScoped<IUnit, UnitRepository>();
            serviceCollection.AddScoped<IVoting, VotingRepository>();
            serviceCollection.AddScoped<IVotingRecipient, VotingRecipientRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork<DataBaseContext>>();

            return serviceCollection;
        }
    }
}
