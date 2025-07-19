using Ardalis.Specification;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.Specifications;

public class PendingMemberByTokenSpec : Specification<PendingMember>
{
    public PendingMemberByTokenSpec(string token)
    {
        Query
            .Where(x => x.Token == token);
    }
}
