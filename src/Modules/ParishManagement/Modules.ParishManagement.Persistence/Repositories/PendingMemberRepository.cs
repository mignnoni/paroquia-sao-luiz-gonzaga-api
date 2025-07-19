using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Persistence.Repositories;

public class PendingMemberRepository(
    ParishManagementDbContext context) : GenericRepository<PendingMember>(context), IPendingMemberRepository
{

}
