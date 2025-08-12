using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.UpdatePassword;

internal sealed class UpdatePasswordCommandHandler(
    IMemberRepository _MemberRepository,
    IIdentityProviderEndpoints _userAccess,
    IUnitOfWork _unitOfWork) : ICommandHandler<UpdatePasswordCommand>
{
    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var spec = new MemberByIdentityProviderIdSpec(request.IdentityId);

        var member = await _MemberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (member is null)
            return Result.Error("Membro não encontrado");

        var result = await _userAccess.UpdatePassword(new UpdatePasswordRequest(
            request.IdentityId,
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword), cancellationToken);

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
