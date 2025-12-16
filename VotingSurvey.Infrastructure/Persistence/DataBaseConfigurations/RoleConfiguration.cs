using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations
{
    public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(r => r.RoleId);
            builder.Property(r => r.RoleId).HasColumnName("RoleId");
            builder.Property(r => r.Name).HasColumnName("Name");
            builder.Property(r => r.Description).HasColumnName("Description");
        }
    }
}
