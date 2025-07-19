namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMemberByToken;

public sealed record GetPendingMemberByTokenResponse(
    string Email,
    string FullName,
    string Token);
