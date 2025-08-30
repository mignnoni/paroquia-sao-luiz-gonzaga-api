using Ardalis.Specification;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.Specifications;

public class NewsSpec : Specification<News>
{
    public NewsSpec(int pageIndex, int pageSize)
    {
        Query
            .OrderByDescending(x => x.CreatedAt)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}
