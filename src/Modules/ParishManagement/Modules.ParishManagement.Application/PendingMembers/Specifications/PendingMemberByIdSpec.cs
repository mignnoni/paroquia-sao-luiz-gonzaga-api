using Ardalis.Specification;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.Specifications;

public class PendingMemberByIdSpec : Specification<PendingMember>
{
    public PendingMemberByIdSpec(PendingMemberId id)
    {
        Query
            .Where(x => x.Id == id);
    }
}
