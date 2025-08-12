using BuildingBlocks.Application;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetMembers;

public sealed record GetMembersQuery(
    int PageIndex,
    int PageSize,
    string? Search = null,
    MemberType? Type = null) : IQuery<List<MemberResponse>>;
