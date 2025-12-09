using Microsoft.EntityFrameworkCore;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.Context
{
    public class DataBaseContext(DbContextOptions<DataBaseContext> context) : DbContext(context)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Community> Communities => Set<Community>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<UserUnit> UserUnits => Set<UserUnit>();
        public DbSet<Voting> Votings => Set<Voting>();
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<VotingRecipient> VotingRecipients => Set<VotingRecipient>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
        }
    }
}
