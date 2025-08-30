using Ardalis.Specification;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.Specifications;

public class NewsByIdSpec : Specification<News>
{
    public NewsByIdSpec(NewsId id, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Id == id)
            .Include(i => i.Files)
            .AsNoTracking(isReadOnly);
    }
}
