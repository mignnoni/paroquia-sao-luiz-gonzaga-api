using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Members.Specifications;
using System.Text.Json;
using System.Text;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;

namespace Modules.ParishManagement.Application.Members.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    IMemberRepository _MemberRepository,
    IIdentityProviderEndpoints _userAccess
    ) : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return Result.Error("As senhas digitadas devem ser iguais");

        byte[] bytes = Convert.FromBase64String(request.Token);
        var jsonStr = Encoding.UTF8.GetString(bytes);

        var dto = JsonSerializer.Deserialize<ResetPasswordTokenDTO>(jsonStr);

        if (dto?.Token is null || dto?.Email is null)
            return Result.Error("Token não informado");

        var spec = new GetMemberByEmailReadOnlySpecification(dto.Email);

        var memberExists = await _MemberRepository.AnyAsync(spec, cancellationToken);

        if (!memberExists)
            return Result.Error("Usuário não encontrado");

        var resetPasswordResult = await _userAccess.ResetPassword(new ResetPasswordRequest(
                dto.Email,
                dto.Token,
                request.NewPassword,
                request.ConfirmNewPassword
            ), cancellationToken);

        return resetPasswordResult;
    }
}
