using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.PendingMembers.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.PendingMembers;

namespace Modules.ParishManagement.Application.PendingMembers.RemovePendingMember;

internal sealed class RemovePendingCommandHandler(
    IPendingMemberRepository _repo,
    IUnitOfWork _uow) : ICommandHandler<RemovePendingMemberCommand>
{
    public async Task<Result> Handle(RemovePendingMemberCommand request, CancellationToken cancellationToken)
    {
        var spec = new PendingMemberByIdSpec(new PendingMemberId(request.Id));

        var pendingMember = await _repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (pendingMember is null)
            return Result.Error("Membro não encontrado");

        _repo.Delete(pendingMember);

        await _uow.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
