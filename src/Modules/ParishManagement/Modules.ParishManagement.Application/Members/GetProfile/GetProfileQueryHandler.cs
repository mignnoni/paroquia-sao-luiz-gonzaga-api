using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Members.GetProfile;

internal class GetProfileQueryHandler(
    IMemberRepository _memberRepository) : IQueryHandler<GetProfileQuery, ProfileResponse>
{
    public async Task<Result<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var spec = new MemberByIdentityProviderIdSpec(request.IdentityId, isReadOnly: true);

        var member = await _memberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (member is null)
            return Result.Error("Membro não encontrado");

        return new ProfileResponse(
            member.Id.Value,
            member.FullName,
            member.Email,
            member.Type);
    }
}
