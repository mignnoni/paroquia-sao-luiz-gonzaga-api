
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.Specifications;

public sealed class GetAllMembersPaginatedReadOnlySpecification : Specification<Member>
{
    public GetAllMembersPaginatedReadOnlySpecification(int pageIndex, int pageSize, string? search, MemberType? type)
    {
        Query
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (search != null)
        {
            Query
                .Where(x => EF.Functions.ILike(x.FullName, $"%{search}%") || EF.Functions.ILike(x.Email, $"%{search}%"));
        }

        if (type != null)
        {
            Query
                .Where(x => x.Type == type);
        }

        Query
            .OrderBy(o => o.FullName)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
    }
}
