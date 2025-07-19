using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.PendingMembers.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.PendingMembers.GetPendingMembers;

internal sealed class GetPendingMembersQueryHandler(
    IPendingMemberRepository _repo) : IQueryHandler<GetPendingMembersQuery, List<PendingMemberResponse>>
{
    public async Task<Result<List<PendingMemberResponse>>> Handle(GetPendingMembersQuery request, CancellationToken cancellationToken)
    {
        var spec = new PendingMemberPaginatedReadOnlySpec(request.PageIndex, request.PageSize);

        var pendingMembers = await _repo.ListAsync(spec, cancellationToken);

        if (pendingMembers is null)
            return Enumerable.Empty<PendingMemberResponse>().ToList();

        return pendingMembers.Select(s => new PendingMemberResponse(
            s.Id.Value,
            s.Email,
            s.FullName,
            s.Token,
            s.CreatedAt)).ToList();
    }
}
