using System;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Masses;

namespace Modules.ParishManagement.Persistence.Repositories;

public class MassLocationRepository(
    ParishManagementDbContext context) : GenericRepository<MassLocation>(context), IMassLocationRepository
{

}
