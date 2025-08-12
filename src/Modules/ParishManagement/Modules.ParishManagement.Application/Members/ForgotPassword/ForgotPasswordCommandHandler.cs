using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Members.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IMemberRepository _MemberRepository,
    IIdentityProviderEndpoints _userAccess) : ICommandHandler<ForgotPasswordCommand, PasswordForgottenResponse>
{
    public async Task<Result<PasswordForgottenResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetMemberByEmailReadOnlySpecification(request.Email);

        var memberExists = await _MemberRepository.AnyAsync(spec, cancellationToken);

        if (!memberExists)
            return Result.Error("Usuário não encontrado");

        var forgotPasswordResult = await _userAccess.ForgotPassword(new ForgotPasswordRequest(request.Email), cancellationToken);

        return forgotPasswordResult.Map(x => new PasswordForgottenResponse(x.Email, x.Token));
    }
}
