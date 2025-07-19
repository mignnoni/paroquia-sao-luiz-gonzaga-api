
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Persistence.Repositories;

public class MemberRepository(
    ParishManagementDbContext context) : GenericRepository<Member>(context), IMemberRepository
{

}
