using Ardalis.Specification;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.Specifications;

public class PendingMemberPaginatedReadOnlySpec : Specification<PendingMember>
{
    public PendingMemberPaginatedReadOnlySpec(int pageIndex, int pageSize)
    {
        Query
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
    }
}
