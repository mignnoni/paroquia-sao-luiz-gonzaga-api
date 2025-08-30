using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Persistence.Repositories;

public class NewsRepository(
    ParishManagementDbContext context) : GenericRepository<News>(context), INewsRepository
{

}
