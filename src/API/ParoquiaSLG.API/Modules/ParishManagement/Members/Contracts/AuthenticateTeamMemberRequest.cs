namespace ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;

public sealed record AuthenticateMemberRequest(string Email, string Password);
