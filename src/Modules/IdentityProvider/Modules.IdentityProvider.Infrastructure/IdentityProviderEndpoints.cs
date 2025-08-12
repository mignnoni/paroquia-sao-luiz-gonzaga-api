using Ardalis.Result;
using MediatR;
using Modules.IdentityProvider.Application.CreateUser;
using Modules.IdentityProvider.Application.DeleteUser;
using Modules.IdentityProvider.Application.ForgotPassword;
using Modules.IdentityProvider.Application.Login;
using Modules.IdentityProvider.Application.Permissions.GetRolePermissions;
using Modules.IdentityProvider.Application.ResetPassword;
using Modules.IdentityProvider.Application.UpdatePassword;
using Modules.IdentityProvider.Application.UpdateUser;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.IdentityProvider.Endpoints.PublicAPI.Responses;

namespace Modules.IdentityProvider.Infrastructure;

internal sealed class IdentityProviderEndpoints(ISender sender) : IIdentityProviderEndpoints
{
    private readonly ISender _sender = sender;

    public async Task<Result<UserCreated>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateUserCommand(
            request.FullName,
            request.Email,
            request.Password,
            request.Role), cancellationToken);

        return result.Map(resp => new UserCreated(resp.Id, resp.FullName, resp.Email));
    }

    public Task<Result> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return _sender.Send(new UpdateUserCommand(
            request.CurrentUserId,
            request.UserId,
            request.Name,
            request.OldRole,
            request.NewRole), cancellationToken);
    }

    public async Task<Result<ForgottenPasswordResponse>> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ForgotPasswordCommand(request.Email), cancellationToken);

        return result.Map(resp => new ForgottenPasswordResponse(resp.Email, resp.Token));
    }

    public async Task<Result<UserAuthenticatedResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);

        return result.Map(response => new UserAuthenticatedResponse(
                    new UserAuthenticated(response.User.FullName, response.User.Email),
                    response.Token));
    }

    public Task<Result<HashSet<string>>> GetPermissionsByRole(string roleName, CancellationToken cancellationToken)
    {
        return _sender.Send(new GetRolePermissionsQuery(roleName), cancellationToken);
    }

    public Task<Result> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        return _sender.Send(new ResetPasswordCommand(
            request.Email,
            request.Token, 
            request.NewPassword, 
            request.ConfirmNewPassword), cancellationToken);
    }

    public Task<Result> DeleteUser(string id, CancellationToken cancellationToken = default)
    {
        return _sender.Send(new DeleteUserCommand(id), cancellationToken);
    }

    public Task<Result> UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        return _sender.Send(new UpdatePasswordCommand(
            request.UserId,
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword), cancellationToken);
    }
}
