using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("User", "dbo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("UserId");
        b.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        b.Property<DateTimeOffset>("CreatedAt").HasColumnName("CreatedAt");

        // PersonName: multi-field VO -> keep as owned/complex
        b.OwnsOne(x => x.Name, n =>
        {
            n.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
            n.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        });

        // EmailAddress: single-field VO -> map with ValueConverter to a scalar column
        b.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(256)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value)
            )
            .IsRequired();
    }
}
