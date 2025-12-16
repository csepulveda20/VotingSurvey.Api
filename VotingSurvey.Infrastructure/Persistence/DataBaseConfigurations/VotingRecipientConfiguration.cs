// C#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.Configurations
{
    public class VotingRecipientConfiguration : IEntityTypeConfiguration<VotingRecipient>
    {
        public void Configure(EntityTypeBuilder<VotingRecipient> builder)
        {
            builder.ToTable("VotingRecipients");
            builder.HasKey(vr => new { vr.VotingId, vr.UserId });

            builder.Property(vr => vr.CreatedAt).IsRequired();

            builder.HasOne<Voting>()
                   .WithMany()
                   .HasForeignKey(vr => vr.VotingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(vr => vr.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}