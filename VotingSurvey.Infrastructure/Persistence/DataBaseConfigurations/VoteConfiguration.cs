using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;
using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

internal class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> b)
    {
        b.ToTable("Vote");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("VoteId");
        b.Property(x => x.VotingId).HasColumnName("VotingId");
        b.Property(x => x.UserId).HasColumnName("UserId");
        b.Property(x => x.Option).HasColumnName("OptionValue").HasConversion<byte>();
        b.Property(x => x.CreatedAt).HasColumnName("CreatedAt");
        b.Property(x => x.Confirmed).HasColumnName("Confirmed");
        b.Property(x => x.ConfirmedAt).HasColumnName("ConfirmedAt");

        b.HasIndex(x => new { x.VotingId, x.UserId }).IsUnique();

        b.HasOne<Voting>()
            .WithMany()
            .HasForeignKey(x => x.VotingId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
