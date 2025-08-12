using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.Members;

namespace Modules.ParishManagement.Application.Members.UpdateMember;

internal sealed class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IIdentityProviderEndpoints _userAccess;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberCommandHandler(
        IMemberRepository memberRepository,
        IIdentityProviderEndpoints userAccess,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _userAccess = userAccess;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(request.MemberType))
            return Result.Error("O tipo de usuário informado não existe");

        var spec = new GetMemberByIdSpecification(new MemberId(request.Id));
        var member = await _memberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (member is null)
            return Result.Error("Membro não encontrado");

        bool updateIdentityUser = request.Name != member.FullName || request.MemberType != member.Type;

        if (!updateIdentityUser)
            return Result.Error("Os dados do usuário já estão atualizados");

        bool updateMemberType = request.MemberType != member.Type;

        member.SetName(request.Name);

        var result = await _userAccess.UpdateUser(new UpdateUserRequest(
            request.CurrentUserIdentityProviderId,
            member.IdentityProviderId,
            request.Name,
            MemberTypeDict.GetRoleName(member.Type),
            updateMemberType ? MemberTypeDict.GetRoleName(request.MemberType) : null),
            cancellationToken);

        if (updateMemberType)
        {
            member.SetType(request.MemberType);
        }

        if (!result.IsSuccess)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}