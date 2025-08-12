using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.Identity;
using Modules.IdentityProvider.Application.Interfaces;
using Modules.IdentityProvider.Domain.Users;
using System.Net;

namespace Modules.IdentityProvider.Application.Login
{
    internal sealed class LoginCommandHandler(
            IJwtProvider jwtProvider,
            SignInManager<User> signInManager,
            UserManager<User> userManager
        ) : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (!result.Succeeded)
                return Result.Invalid(new ValidationError
                {
                    ErrorCode = HttpStatusCode.Unauthorized.ToString(),
                    ErrorMessage = "E-mail ou senha incorretos",
                    Identifier = "InvalidCredentials",
                    Severity = ValidationSeverity.Error
                });

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.NotFound();

            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtProvider.Generate(claims.ToList(), roles.ToList());

            return new LoginResponse(new LoginUserResponse(user.FullName, request.Email), token);
        }
    }
}
