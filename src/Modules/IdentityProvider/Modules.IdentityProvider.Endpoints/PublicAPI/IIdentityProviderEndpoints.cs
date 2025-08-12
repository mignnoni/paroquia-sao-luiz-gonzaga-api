using Ardalis.Result;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.IdentityProvider.Endpoints.PublicAPI.Responses;

namespace Modules.IdentityProvider.Endpoints.PublicAPI;

public interface IIdentityProviderEndpoints
{
    Task<Result<UserCreated>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<UserAuthenticatedResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<ForgottenPasswordResponse>> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result<HashSet<string>>> GetPermissionsByRole(string roleName, CancellationToken cancellationToken = default);
    Task<Result> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteUser(string id, CancellationToken cancellationToken = default);
    Task<Result> UpdatePassword(UpdatePasswordRequest request, CancellationToken cancellationToken = default);
}
