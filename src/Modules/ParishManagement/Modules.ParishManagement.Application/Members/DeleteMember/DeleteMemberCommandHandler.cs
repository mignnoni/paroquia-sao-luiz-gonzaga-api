using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.DeleteMember;

internal sealed class DeleteMemberCommandHandler(
    IMemberRepository _memberRepository,
    IIdentityProviderEndpoints _userAccess,
    IUnitOfWork _unitOfWork) : ICommandHandler<DeleteMemberCommand>
{
    public async Task<Result> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == request.RequestedBy)
        {
            return Result.Error("Você não pode excluir sua própria conta");
        }

        var spec = new GetMemberByIdSpecification(new MemberId(request.Id));

        var member = await _memberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (member is null)
            return Result.NotFound("Membro não encontrado");

        member.SetDeleted();

        var userDeleteResult = await _userAccess.DeleteUser(member.IdentityProviderId.ToString(), cancellationToken);

        if (userDeleteResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return userDeleteResult;
    }
}
