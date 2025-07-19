using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Persistence.Configurations;

public sealed class PendingMemberConfig : IEntityTypeConfiguration<PendingMember>
{
    public void Configure(EntityTypeBuilder<PendingMember> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, value => new PendingMemberId(value));

        builder
            .Property(x => x.MemberType)
            .HasConversion<int>()
            .IsRequired();

        builder
            .Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);

        builder
            .Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Token)
            .HasMaxLength(200)
            .IsRequired();
    }
}

