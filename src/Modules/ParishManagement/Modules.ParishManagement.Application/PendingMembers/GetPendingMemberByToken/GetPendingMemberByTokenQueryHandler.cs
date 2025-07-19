using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.PendingMembers.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMemberByToken;

internal sealed class GetPendingMemberByTokenQueryHandler(
    IPendingMemberRepository _repo) : IQueryHandler<GetPendingMemberByTokenQuery, GetPendingMemberByTokenResponse>
{
    public async Task<Result<GetPendingMemberByTokenResponse>> Handle(GetPendingMemberByTokenQuery request, CancellationToken cancellationToken)
    {
        var spec = new PendingMemberByTokenReadOnlySpec(request.Token);

        var pendingMember = await _repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (pendingMember is null)
            return Result.Error("Token inválido ou já utilizado");

        return new GetPendingMemberByTokenResponse(
            pendingMember.Email,
            pendingMember.FullName,
            pendingMember.Token);
    }
}
