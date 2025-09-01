using System;
using Ardalis.Specification;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.Specifications;

public class HighlightedNewsSpec : Specification<News>
{
    public HighlightedNewsSpec(bool filterByDate = false, bool isReadOnly = false)
    {
        Query
            .Where(x => x.Highlight);

        if (filterByDate)
        {
            Query.Where(x => x.HighlightUntil >= DateTime.UtcNow);
        }
    }
}
