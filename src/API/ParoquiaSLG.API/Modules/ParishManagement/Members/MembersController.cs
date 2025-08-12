using ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.ParishManagement.Application.Members.Authenticate;
using Modules.ParishManagement.Application.Members.CreateMember;
using Modules.ParishManagement.Application.Members.DeleteMember;
using Modules.ParishManagement.Application.Members.ForgotPassword;
using Modules.ParishManagement.Application.Members.GetMemberById;
using Modules.ParishManagement.Application.Members.GetMembers;
using Modules.ParishManagement.Application.Members.GetProfile;
using Modules.ParishManagement.Application.Members.ResetPassword;
using Modules.ParishManagement.Application.Members.UpdatePassword;
using Modules.ParishManagement.Application.Members.UpdateProfile;
using Modules.ParishManagement.Domain.Members;
using ParoquiaSLG.API.Configuration.Authorization.Extensions;
using ParoquiaSLG.API.Constants;
using ParoquiaSLG.API.Authorization;
using Modules.ParishManagement.Application.Members.UpdateMember;

namespace ParoquiaSLG.API.Modules.ParishManagement.Members;

[Authorize]
[Route("[controller]")]
[ApiController]
public class MembersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [AllowAnonymous]
    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result> CreateMember(CreateMemberRequest request)
    {
        var command = new CreateMemberCommand(
            request.Token,
            request.Name,
            request.Password,
            request.ConfirmPassword);

        return await _sender.Send(command);
    }

    [HasPermission(ParishManagementPermissions.ReadMember)]
    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<List<MemberResponse>>> GetAll([FromQuery] int pageIndex = 0, int pageSize = 10, string? search = null, MemberType? memberType = null)
    {
        return await _sender.Send(new GetMembersQuery(pageIndex, pageSize, search, memberType));
    }

    [HasPermission(ParishManagementPermissions.ReadMember)]
    [HttpGet("{id}")]
    [TranslateResultToActionResult]
    public async Task<Result<MemberByIdResponse>> GetById(Guid id)
    {
        return await _sender.Send(new GetMemberByIdQuery(id));
    }

    [HasPermission(ParishManagementPermissions.DeleteMember)]
    [HttpDelete]
    [TranslateResultToActionResult]
    public async Task<Result> DeleteMember([FromQuery] Guid id)
    {
        var requestedBy = HttpContext.User.GetMemberId();
        var isAdmin = HttpContext.User.IsInRole(Roles.Admin);

        return await _sender.Send(new DeleteMemberCommand(id, requestedBy, isAdmin));
    }

    [HasPermission(ParishManagementPermissions.UpdateMember)]
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    public async Task<Result> UpdateMember(Guid id, [FromBody] UpdateMemberRequest request)
    {
        var currentUserIdentityProviderId = HttpContext.User.GetUserId();

        return await _sender.Send(new UpdateMemberCommand(
            currentUserIdentityProviderId,
            id,
            request.Name,
            request.MemberType));
    }

    [HasPermission(ParishManagementPermissions.ReadProfile)]
    [HttpGet("profile")]
    [TranslateResultToActionResult]
    public async Task<Result<ProfileResponse>> GetProfile()
    {
        var identityId = HttpContext.User.GetUserId();

        return await _sender.Send(new GetProfileQuery(identityId));
    }

    [HasPermission(ParishManagementPermissions.UpdateProfile)]
    [HttpPut("profile")]
    [TranslateResultToActionResult]
    public async Task<Result> Edit([FromBody] UpdateProfileRequest request)
    {
        var identityId = HttpContext.User.GetUserId();

        return await _sender.Send(new UpdateProfileCommand(
            identityId,
            request.Name));
    }

    [HasPermission(ParishManagementPermissions.UpdateProfile)]
    [HttpPut("password")]
    [TranslateResultToActionResult]
    public async Task<Result> UpdatePassword(UpdatePasswordRequest request)
    {
        var identityId = HttpContext.User.GetUserId();

        return await _sender.Send(new UpdatePasswordCommand(
            identityId,
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [TranslateResultToActionResult]
    public async Task<Result<AuthenticateMemberResponse>> Login(AuthenticateMemberRequest request)
    {
        return await _sender.Send(new AuthenticateMemberCommand(
                request.Email,
                request.Password
            ));
    }

    [AllowAnonymous]
    [HttpPost("forgotPassword")]
    [TranslateResultToActionResult]
    public async Task<Result<PasswordForgottenResponse>> ForgotPassword(ForgotPasswordRequest request)
    {
        return await _sender.Send(new ForgotPasswordCommand(request.Email));
    }

    [AllowAnonymous]
    [HttpPost("resetPassword")]
    [TranslateResultToActionResult]
    public async Task<Result> ResetPassword(ResetPasswordRequest request)
    {
        return await _sender.Send(new ResetPasswordCommand(
                request.Token,
                request.NewPassword,
                request.ConfirmNewPassword
            ));
    }
}
