using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Persistence.Repositories;

public class OtherScheduleRepository(
    ParishManagementDbContext context) : GenericRepository<OtherSchedule>(context), IOtherScheduleRepository
{
}