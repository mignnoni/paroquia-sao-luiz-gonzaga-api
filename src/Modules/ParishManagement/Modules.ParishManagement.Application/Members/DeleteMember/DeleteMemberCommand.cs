using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.DeleteMember;

public sealed record DeleteMemberCommand(Guid Id, Guid RequestedBy, bool IsAdmin) : ICommand;
