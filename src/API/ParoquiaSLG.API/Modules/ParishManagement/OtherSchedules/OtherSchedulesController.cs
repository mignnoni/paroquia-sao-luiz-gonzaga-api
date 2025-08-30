using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.ParishManagement.Application.OtherSchedules.CreateOtherSchedule;
using Modules.ParishManagement.Application.OtherSchedules.DeleteOtherSchedule;
using Modules.ParishManagement.Application.OtherSchedules.GetOtherScheduleById;
using Modules.ParishManagement.Application.OtherSchedules.GetOtherSchedules;
using Modules.ParishManagement.Application.OtherSchedules.UpdateOtherSchedule;
using ParoquiaSLG.API.Authorization;
using ParoquiaSLG.API.Modules.ParishManagement.OtherSchedules.Contracts;
using Modules.ParishManagement.Domain.OtherSchedules;
using OtherSchedulesResponse = Modules.ParishManagement.Application.OtherSchedules.GetOtherSchedules.OtherScheduleResponse;
using Modules.ParishManagement.Application.OtherSchedules.GetFirstOtherScheduleByType;

namespace ParoquiaSLG.API.Modules.ParishManagement.OtherSchedules;

[Route("[controller]")]
[ApiController]
[TranslateResultToActionResult]
public class OtherSchedulesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HasPermission(ParishManagementPermissions.CreateOtherSchedule)]
    [HttpPost]
    public async Task<Result> CreateOtherSchedule([FromForm] CreateOtherScheduleRequest request)
    {
        return await _sender.Send(new CreateOtherScheduleCommand(request.Title, request.Content, request.Type));
    }

    [HasPermission(ParishManagementPermissions.ReadOtherSchedule)]
    [HttpGet]
    public async Task<Result<List<OtherSchedulesResponse>>> GetAll(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] ScheduleType? type = null)
    {
        return await _sender.Send(new GetOtherSchedulesQuery(pageIndex, pageSize, type));
    }

    [HasPermission(ParishManagementPermissions.ReadOtherSchedule)]
    [HttpGet("{id}")]
    public async Task<Result<OtherScheduleByIdResponse>> GetById(Guid id)
    {
        return await _sender.Send(new GetOtherScheduleByIdQuery(id));
    }

    [HasPermission(ParishManagementPermissions.UpdateOtherSchedule)]
    [HttpPut("{id}")]
    public async Task<Result> UpdateOtherSchedule(Guid id, [FromForm] UpdateOtherScheduleRequest request)
    {
        return await _sender.Send(new UpdateOtherScheduleCommand(id, request.Title, request.Content, request.Type));
    }

    [HasPermission(ParishManagementPermissions.DeleteOtherSchedule)]
    [HttpDelete("{id}")]
    public async Task<Result> DeleteOtherSchedule(Guid id)
    {
        return await _sender.Send(new DeleteOtherScheduleCommand(id));
    }

    [HasPermission(ParishManagementPermissions.ReadOtherSchedule)]
    [HttpGet("first/{type}")]
    public async Task<Result<OtherScheduleByIdResponse>> GetFirstByType(ScheduleType type)
    {
        return await _sender.Send(new GetFirstOtherScheduleByTypeQuery(type));
    }
}
