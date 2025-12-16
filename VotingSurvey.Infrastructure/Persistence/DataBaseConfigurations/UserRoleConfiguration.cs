using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations
{
    public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.Property(ur => ur.RoleId).HasColumnName("RoleId");
            builder.Property(ur => ur.UserId).HasColumnName("UserId");
            builder.Property(ur => ur.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
