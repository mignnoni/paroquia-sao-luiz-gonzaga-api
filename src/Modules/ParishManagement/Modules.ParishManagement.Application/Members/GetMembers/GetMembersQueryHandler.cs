using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetMembers;

internal sealed class GetMembersQueryHandler(IMemberRepository _MemberRepository)
    : IQueryHandler<GetMembersQuery, List<MemberResponse>>
{
    public async Task<Result<List<MemberResponse>>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllMembersPaginatedReadOnlySpecification(request.PageIndex, request.PageSize, request.Search, request.Type);

        List<Member> Members = await _MemberRepository.ListAsync(spec, cancellationToken);

        List<MemberResponse> MembersReponse = [.. Members
            .Select(member => new MemberResponse(
                    member.Id.Value,
                    member.FullName,
                    member.Email,
                    MemberTypeDict.GetMemberTypeTranslated(member.Type)))];

        return MembersReponse;
    }
}
