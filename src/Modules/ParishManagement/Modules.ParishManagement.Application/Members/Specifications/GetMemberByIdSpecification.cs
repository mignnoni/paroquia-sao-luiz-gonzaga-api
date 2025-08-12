using Ardalis.Specification;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public class GetMemberByIdSpecification : Specification<Member>
{
    public GetMemberByIdSpecification(MemberId id, MemberType? memberType = null)
    {
        Query
            .Where(x => !x.IsDeleted)
            .Where(x => x.Id == id);

        if (memberType is not null)
        {
            Query.Where(x => x.Type == memberType);
        }
    }
}

