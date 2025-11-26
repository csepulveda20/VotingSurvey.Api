using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

public sealed class VotingConfiguration : IEntityTypeConfiguration<Voting>
{
    public void Configure(EntityTypeBuilder<Voting> b)
    {
        b.ToTable("Voting", "dbo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("VotingId");
        b.Property(x => x.CommunityId).HasColumnName("CommunityId");
        b.Property(x => x.CreatedByUserId).HasColumnName("CreatedByUserId");
        b.Property(x => x.IsPublished).HasColumnName("IsPublished");
        b.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
        b.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt");

        b.Property(x => x.Title).HasColumnName("Title").HasMaxLength(200).IsRequired();
        b.Property(x => x.QuestionDescription).HasColumnName("QuestionDescription").HasMaxLength(1000).IsRequired();

        b.OwnsOne(x => x.Window, w =>
        {
            w.Property(p => p.StartsAt).HasColumnName("StartAt");
            w.Property(p => p.EndsAt).HasColumnName("EndAt");
        });

        b.HasOne<Community>()
            .WithMany()
            .HasForeignKey(x => x.CommunityId)
            .OnDelete(DeleteBehavior.NoAction);
        b.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
