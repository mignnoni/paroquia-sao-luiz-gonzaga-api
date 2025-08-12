using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Persistence.Configurations;

public class MemberConfig : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(x => x.Value, value => new MemberId(value));

        builder
            .Property(x => x.IdentityProviderId)
            .IsRequired();

        builder
            .Property(x => x.FullName)
            .HasMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.Email)
            .HasMaxLength(80)
            .IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder
            .Property(x => x.IsDeleted)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
