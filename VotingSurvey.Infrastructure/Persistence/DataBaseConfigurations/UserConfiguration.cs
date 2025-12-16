using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("User");
        b.HasKey(u => u.Id);
        b.Property(u => u.Id).HasColumnName("UserId");

        b.OwnsOne(u => u.Name, nb =>
        {
            nb.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(100);
            nb.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(100);
        });

        b.OwnsOne(u => u.Email, eb =>
        {
            eb.Property(e => e.Value).HasColumnName("Email").HasMaxLength(256);
        });

        b.Navigation(u => u.Name).IsRequired();
        b.Navigation(u => u.Email).IsRequired();
    }
}
