using Ardalis.Specification;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public class MemberByIdentityProviderIdSpec : Specification<Member>
{
    public MemberByIdentityProviderIdSpec(Guid identityProviderId, bool isReadOnly = false)
    {
        Query
            .AsNoTracking(isReadOnly)
            .Where(x => !x.IsDeleted)
            .Where(x => x.IdentityProviderId == identityProviderId);
    }
}
