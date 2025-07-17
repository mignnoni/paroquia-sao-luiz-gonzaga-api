using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Modules.IdentityProvider.Domain.Roles;

namespace Modules.IdentityProvider.Persistence.Configurations;

public sealed class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(80).IsRequired();
    }
}
