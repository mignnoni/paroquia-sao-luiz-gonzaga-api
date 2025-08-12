using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.CreateMember;

public sealed record CreateMemberCommand(
    string Token,
    string Name,
    string Password,
    string ConfirmPassword) : ICommand;