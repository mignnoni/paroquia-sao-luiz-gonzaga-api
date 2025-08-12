using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.PendingMembers.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.CreateMember;

internal class CreateMemberCommandHandler(
    IIdentityProviderEndpoints _identityProviderEndpoints,
    IMemberRepository _repo,
    IPendingMemberRepository _pendingMemberRepo,
    IUnitOfWork _uow)
    : ICommandHandler<CreateMemberCommand>
{
    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var spec = new PendingMemberByTokenSpec(request.Token);

        var pendingMember = await _pendingMemberRepo.FirstOrDefaultAsync(spec, cancellationToken);

        if (pendingMember is null)
            return Result.Error("Token de registro inválido");

        var role = MemberTypeDict.GetRoleName(pendingMember.MemberType);

        if (role is null)
            return Result.Error("Tipo de membro não encontrado");

        var identityProviderResult = await _identityProviderEndpoints
            .CreateUser(new CreateUserRequest(
                pendingMember.FullName,
                pendingMember.Email,
                request.Password,
                role), cancellationToken);


        if (identityProviderResult.IsSuccess)
        {
            var teamMember = Member.Create(
                new MemberId(Guid.NewGuid()),
                identityProviderResult.Value.Id,
                pendingMember.FullName,
                pendingMember.Email,
                pendingMember.MemberType);

            _repo.Add(teamMember);

            _pendingMemberRepo.Delete(pendingMember);

            await _uow.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        return Result.Error(identityProviderResult.Errors.First());
    }
}
