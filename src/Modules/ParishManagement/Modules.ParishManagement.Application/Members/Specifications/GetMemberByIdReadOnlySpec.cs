using Ardalis.Specification;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public class GetMemberByIdReadOnlySpec : Specification<Member>
{
    public GetMemberByIdReadOnlySpec(MemberId id)
    {
        Query
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => x.Id == id);
    }
}
