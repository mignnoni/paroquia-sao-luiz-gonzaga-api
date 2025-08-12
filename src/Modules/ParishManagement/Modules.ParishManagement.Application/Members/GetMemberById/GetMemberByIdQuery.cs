using BuildingBlocks.Application;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetMemberById;

public sealed record GetMemberByIdQuery(Guid Id) : IQuery<MemberByIdResponse>;
