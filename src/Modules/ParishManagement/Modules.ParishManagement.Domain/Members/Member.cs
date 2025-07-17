using Ardalis.Result;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Members;

public class Member : Entity<MemberId>
{
    private Member(MemberId id, string fullName, string email, MemberType type) : base(id)
    {
        FullName = fullName;
        Email = email;
        Type = type;
        IsDeleted = false;
    }

    // EF Core
    private Member() { }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public MemberType Type { get; private set; }
    public bool IsDeleted { get; private set; }

    public static Member Create(MemberId id, string fullName, string email, MemberType type)
    {
        return new Member(id, fullName, email, type);
    }

    public void SetName(string fullName)
    {
        FullName = fullName;
    }

    public Result SetDeleted()
    {
        if (IsDeleted)
            return Result.Error("O usuário já está deletado");

        IsDeleted = true;
        return Result.Success();
    }
}
