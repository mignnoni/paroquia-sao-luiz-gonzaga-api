using Ardalis.Specification;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.Specifications;

public class PendingMemberByTokenReadOnlySpec : Specification<PendingMember>
{
    public PendingMemberByTokenReadOnlySpec(string token)
    {
        Query
            .AsNoTracking()
            .Where(x => x.Token == token);
    }
}
