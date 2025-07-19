using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.PendingMembers.RemovePendingMember;

public sealed record RemovePendingMemberCommand(Guid Id) : ICommand;