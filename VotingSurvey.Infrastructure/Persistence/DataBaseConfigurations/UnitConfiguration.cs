using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> b)
    {
        b.ToTable("Unit");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("UnitId");
        b.Property(x => x.CommunityId).HasColumnName("CommunityId");
        b.Property(x => x.Code).HasColumnName("Code").HasMaxLength(50).IsRequired();
        b.Property(x => x.Description).HasColumnName("Description").HasMaxLength(200);
        b.HasIndex(x => new { x.CommunityId, x.Code }).IsUnique();
        b.HasOne<Community>()
            .WithMany()
            .HasForeignKey(x => x.CommunityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
