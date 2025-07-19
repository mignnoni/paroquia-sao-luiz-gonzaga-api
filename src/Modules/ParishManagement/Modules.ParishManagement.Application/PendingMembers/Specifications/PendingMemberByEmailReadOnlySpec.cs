using Ardalis.Specification;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.Specifications;

public class PendingMemberByEmailReadOnlySpec : Specification<PendingMember>
{
    public PendingMemberByEmailReadOnlySpec(string email)
    {
        Query
            .AsNoTracking()
            .Where(x => x.Email == email);
    }
}
