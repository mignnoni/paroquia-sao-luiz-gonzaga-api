namespace ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;

public sealed record CreateMemberRequest(
    string Token,
    string Name,
    string Password,
    string ConfirmPassword);