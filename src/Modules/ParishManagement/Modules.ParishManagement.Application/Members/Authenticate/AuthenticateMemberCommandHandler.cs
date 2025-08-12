using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Members.Authenticate;

internal class AuthenticateMemberCommandHandler(
    IIdentityProviderEndpoints userAccess,
    IMemberRepository MemberRepository) 
    : ICommandHandler<AuthenticateMemberCommand, AuthenticateMemberResponse>
{
    private readonly IIdentityProviderEndpoints _userAccess = userAccess;
    private readonly IMemberRepository _MemberRepository = MemberRepository;

    public async Task<Result<AuthenticateMemberResponse>> Handle(AuthenticateMemberCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetMemberByEmailReadOnlySpecification(request.Email);

        var Member = await _MemberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (Member == null)
            return Result<AuthenticateMemberResponse>.NotFound();

        var result = await _userAccess.Login(new LoginRequest(request.Email, request.Password), cancellationToken);

        return result.Map(result =>
            new AuthenticateMemberResponse(
                new AuthenticatedMember(
                    result.User.FullName,
                    result.User.Email),
                    result.Token));
    }
}
