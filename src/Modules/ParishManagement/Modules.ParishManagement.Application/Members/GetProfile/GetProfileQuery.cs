using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.GetProfile;

public sealed record GetProfileQuery(Guid IdentityId) : IQuery<ProfileResponse>;