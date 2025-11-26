using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

internal class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> b)
    {
        b.ToTable("Community");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("CommunityId");
        b.Property(x => x.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
        b.Property(x => x.Address).HasColumnName("Address").HasMaxLength(300);
        b.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
        b.Property<DateTimeOffset>("CreatedAt").HasColumnName("CreatedAt");
    }
}
