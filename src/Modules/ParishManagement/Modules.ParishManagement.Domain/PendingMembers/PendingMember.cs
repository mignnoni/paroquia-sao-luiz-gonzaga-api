using BuildingBlocks.Domain;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Domain.PendingMembers;

public sealed class PendingMember : Entity<PendingMemberId>
{
    private PendingMember(PendingMemberId id, string fullName, string email, string token, MemberType memberType)
        : base(id)
    {
        FullName = fullName;
        Email = email;
        MemberType = memberType;
        Token = token;
    }

    private PendingMember()
    {

    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public MemberType MemberType { get; set; }

    public static PendingMember Create(PendingMemberId id, string fullName, string email, string token, MemberType memberType)
    {
        return new PendingMember(id, fullName, email, token, memberType);
    }

}
