using Ardalis.Result;

namespace Modules.IdentityProvider.Domain.Users.Interfaces
{
    public interface ICreateUserService
    {
        Task<Result<Guid>> CreateUser(string fullName, string email, string password, string role);
    }
}
