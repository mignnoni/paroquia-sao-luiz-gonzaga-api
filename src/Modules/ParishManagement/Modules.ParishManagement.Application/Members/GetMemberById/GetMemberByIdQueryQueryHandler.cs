using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.GetMemberById;

internal sealed class GetMemberByIdQueryQueryHandler(IMemberRepository _MemberRepository)
    : IQueryHandler<GetMemberByIdQuery, MemberByIdResponse>
{
    public async Task<Result<MemberByIdResponse>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetMemberByIdReadOnlySpec(new MemberId(request.Id));

        var Member = await _MemberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (Member is null)
            return Result<MemberByIdResponse>.Error("Usuário não encontrado");

        return new MemberByIdResponse(
            Member.Id.Value,
            Member.Type,
            Member.FullName,
            Member.Email);
    }
}
