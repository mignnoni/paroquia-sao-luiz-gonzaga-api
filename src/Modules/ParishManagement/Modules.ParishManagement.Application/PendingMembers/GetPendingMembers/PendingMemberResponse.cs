namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMembers;

public sealed record PendingMemberResponse(
    Guid Id,
    string Email,
    string FullName,
    string Token,
    DateTime CreatedOn);
