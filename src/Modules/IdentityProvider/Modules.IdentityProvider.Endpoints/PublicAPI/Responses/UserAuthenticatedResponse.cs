namespace Modules.IdentityProvider.Endpoints.PublicAPI.Responses;

public sealed record UserAuthenticatedResponse
{
    public UserAuthenticatedResponse(UserAuthenticated user, string token)
    {
        User = user;
        Token = token;
    }
    public UserAuthenticated User { get; set; }
    public string Token { get; set; }
}

public sealed record UserAuthenticated
{
    public UserAuthenticated(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
    }

    public string FullName { get; set; }
    public string Email { get; set; }
}
