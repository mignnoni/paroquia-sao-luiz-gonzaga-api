using System.Security.Claims;

namespace Modules.IdentityProvider.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(List<Claim> claims, List<string> roles);
    }
}
