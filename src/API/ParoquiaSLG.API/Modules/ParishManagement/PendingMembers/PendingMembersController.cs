using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.ParishManagement.Application.PendingMembers.AddPendingMember;
using Modules.ParishManagement.Application.PendingMembers.GetPendingMemberByToken;
using Modules.ParishManagement.Application.PendingMembers.GetPendingMembers;
using Modules.ParishManagement.Application.PendingMembers.RemovePendingMember;
using ParoquiaSLG.API.Authorization;
using ParoquiaSLG.API.Constants;
using ParoquiaSLG.API.Modules.ParishManagement.PendingMembers.Contracts;

namespace ParoquiaSLG.API.Modules.ParishManagement.PendingMembers;


[Route("[controller]")]
[ApiController]
public class PendingMembersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HasPermission(ParishManagementPermissions.CreatePendingMember)]
    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<AddPendingMemberResponse>> CreatePendingMember(AddPendingMemberRequest request)
    {
        return await _sender.Send(new AddPendingMemberCommand(
                request.FullName,
                request.Email,
                request.MemberType));
    }

    [HasPermission(ParishManagementPermissions.ReadPendingMember)]
    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<List<PendingMemberResponse>>> GetAll([FromQuery] int pageIndex, int pageSize)
    {
        var user = HttpContext.User;

        return await _sender.Send(new GetPendingMembersQuery(pageIndex, pageSize));
    }

    [AllowAnonymous]
    [HttpGet("getByToken")]
    [TranslateResultToActionResult]
    public async Task<Result<GetPendingMemberByTokenResponse>> GetByToken([FromQuery] string token)
    {
        return await _sender.Send(new GetPendingMemberByTokenQuery(token));
    }

    [HasPermission(ParishManagementPermissions.DeletePendingMember)]
    [HttpDelete]
    [TranslateResultToActionResult]
    public async Task<Result> Remove([FromQuery] Guid id)
    {
        bool isAdmin = HttpContext.User.IsInRole(Roles.Admin);
        return await _sender.Send(new RemovePendingMemberCommand(id));
    }
}
