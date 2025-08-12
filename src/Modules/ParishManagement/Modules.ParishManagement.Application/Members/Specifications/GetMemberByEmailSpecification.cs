using Ardalis.Specification;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public class GetMemberByEmailReadOnlySpecification : Specification<Member>
{
    public GetMemberByEmailReadOnlySpecification(string email)
    {
        Query
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => x.Email == email);
    }
}
