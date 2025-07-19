using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMemberByToken;

public sealed record GetPendingMemberByTokenQuery(string Token) : IQuery<GetPendingMemberByTokenResponse>;
