using Ardalis.Specification;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public class MemberByEmailReadOnlySpec : Specification<Member>
{
    public MemberByEmailReadOnlySpec(string email)
    {
        Query
            .Where(x => x.Email == email)
            .Where(x => !x.IsDeleted)
            .AsNoTracking();
    }
}
