using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.UpdateProfile;

internal sealed class UpdateProfileCommandHandler(
    IMemberRepository _MemberRepository,
    IIdentityProviderEndpoints _userAccess,
    IUnitOfWork _unitOfWork) : ICommandHandler<UpdateProfileCommand>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var spec = new MemberByIdentityProviderIdSpec(request.IdentityId);

        var member = await _MemberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (member is null)
            return Result.Error("Membro não encontrado");

        bool update = request.Name != member.FullName;

        if (!update)
            return Result.Error("Os dados do usuário já estão atualizados");

        member.SetName(request.Name);

        var result = await _userAccess.UpdateUser(new UpdateUserRequest(
            member.IdentityProviderId,
            member.IdentityProviderId,
            request.Name,
            MemberTypeDict.GetRoleName(member.Type)), cancellationToken);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
