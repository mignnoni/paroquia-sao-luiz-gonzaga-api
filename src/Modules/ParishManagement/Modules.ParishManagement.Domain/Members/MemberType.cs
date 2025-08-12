namespace Modules.ParishManagement.Domain.Members;

public enum MemberType
{
    Member = 0,
    Manager = 1,
    Admin = 2
}

public static class MemberTypeDict
{
    private static readonly Dictionary<MemberType, string> RolesDictionaryTranslated = new()
    {
        { MemberType.Admin, "Administrador" },
        { MemberType.Manager, "Gerente" },
        { MemberType.Member, "Membro" }
    };

    private static readonly Dictionary<MemberType, string> RolesDictionary = new()
    {
        { MemberType.Admin, "admin" },
        { MemberType.Manager, "manager" },
        { MemberType.Member, "member" }
    };

    public static string GetRoleName(MemberType type)
    {
        if (RolesDictionary.TryGetValue(type, out string? value))
        {
            return value;
        }

        throw new ArgumentException("Role não encontrada.");
    }

    public static string GetMemberTypeTranslated(MemberType type)
    {
        if (RolesDictionaryTranslated.TryGetValue(type, out string? value))
        {
            return value;
        }

        throw new ArgumentException("Tipo de membro não encontrado.");
    }

    public static MemberType? GetMemberType(string type)
    {
        var result = RolesDictionary.FirstOrDefault(x => x.Value == type);

        if (string.IsNullOrEmpty(result.Value))
            return null;

        return result.Key;
    }
}
