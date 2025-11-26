using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSurvey.Domain.Entities;

namespace VotingSurvey.Infrastructure.Persistence.DataBaseConfigurations;

public sealed class UserUnitConfiguration : IEntityTypeConfiguration<UserUnit>
{
    public void Configure(EntityTypeBuilder<UserUnit> b)
    {
        b.ToTable("UserUnit", "dbo");
        b.HasKey(x => new { x.UserId, x.UnitId, x.FromDate });
        b.Property(x => x.UserId).HasColumnName("UserId");
        b.Property(x => x.UnitId).HasColumnName("UnitId");
        b.Property(x => x.FromDate).HasColumnName("FromDate");
        b.Property(x => x.ToDate).HasColumnName("ToDate");
        b.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Unit>()
            .WithMany()
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
