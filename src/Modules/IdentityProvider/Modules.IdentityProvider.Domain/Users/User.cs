using Microsoft.AspNetCore.Identity;

namespace Modules.IdentityProvider.Domain.Users;

public class User : IdentityUser<Guid>
{
    public string FullName { get; private set; }
    private User(string fullName, string email)
    {
        this.Id = Guid.NewGuid();
        this.UserName = email;
        this.Email = email;
        this.EmailConfirmed = true;
        this.FullName = fullName;
    }

    private User()
    {

    }

    public static User Create(string fullName, string email)
    {
        return new User(fullName, email);
    }

    public void SetName(string name)
    {
        FullName = name;
    }
}
