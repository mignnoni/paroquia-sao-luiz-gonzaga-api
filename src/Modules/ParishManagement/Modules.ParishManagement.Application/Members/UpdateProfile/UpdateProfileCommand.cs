using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.UpdateProfile;

public sealed record UpdateProfileCommand(
    Guid IdentityId,
    string Name) : ICommand;