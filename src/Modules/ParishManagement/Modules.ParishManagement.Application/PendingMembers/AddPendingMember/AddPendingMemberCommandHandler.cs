using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Application.EventBus;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Application.PendingMembers.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.PendingMembers;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;
using System.Text;

namespace Modules.ParishManagement.Application.PendingMembers.AddPendingMember;

internal class AddPendingMemberCommandHandler(
    IPendingMemberRepository _repo,
    IMemberRepository _memberRepo,
    IUnitOfWork _uow,
    IEventBus _bus) : ICommandHandler<AddPendingMemberCommand, AddPendingMemberResponse>
{
    public async Task<Result<AddPendingMemberResponse>> Handle(AddPendingMemberCommand request, CancellationToken cancellationToken)
    {
        var spec = new PendingMemberByEmailReadOnlySpec(request.Email);

        var pendingMemberExists = await _repo.AnyAsync(spec, cancellationToken);

        if (pendingMemberExists)
            return Result.Error("O e-mail já está cadastrado para um membro pendente");

        var memberSpec = new MemberByEmailReadOnlySpec(request.Email);

        var memberExists = await _memberRepo.AnyAsync(memberSpec, cancellationToken);

        if (memberExists)
            return Result.Error("O e-mail já está cadastrado para um membro da equipe");

        byte[] byteArray = Encoding.UTF8.GetBytes($"{Guid.NewGuid()}{DateTimeOffset.UtcNow}");

        var token = Convert.ToBase64String(byteArray);

        var pendingMember = PendingMember.Create(
            new PendingMemberId(Guid.NewGuid()),
            request.FullName,
            request.Email,
            token,
            request.MemberType);

        _repo.Add(pendingMember);

        await _uow.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new PendingMemberCreatedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            pendingMember.FullName,
            pendingMember.Email,
            token), cancellationToken);

        return new AddPendingMemberResponse(pendingMember.Id.Value, pendingMember.FullName, pendingMember.Email, token, pendingMember.MemberType);
    }
}
