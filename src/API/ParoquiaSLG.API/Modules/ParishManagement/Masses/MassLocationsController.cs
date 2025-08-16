using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.ParishManagement.Application.Masses.Locations.CreateMassLocation;
using Modules.ParishManagement.Application.Masses.Locations.DeleteMassLocation;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocationById;
using Modules.ParishManagement.Application.Masses.Locations.GetMassLocations;
using Modules.ParishManagement.Application.Masses.Locations.UpdateMassLocation;
using Modules.ParishManagement.Application.Masses.Schedules.AddScheduleToLocation;
using Modules.ParishManagement.Application.Masses.Schedules.RemoveScheduleFromLocation;
using Modules.ParishManagement.Application.Masses.Schedules.UpdateSchedule;
using Modules.ParishManagement.Application.Masses.Times.AddTimeToSchedule;
using Modules.ParishManagement.Application.Masses.Times.RemoveTimeFromSchedule;
using Modules.ParishManagement.Application.Masses.Times.UpdateTime;
using ParoquiaSLG.API.Authorization;
using ParoquiaSLG.API.Modules.ParishManagement.Masses.Contracts;

namespace ParoquiaSLG.API.Modules.ParishManagement.Masses;

[Route("[controller]")]
[ApiController]
[TranslateResultToActionResult]
public class MassLocationsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HasPermission(ParishManagementPermissions.CreateMassLocation)]
    [HttpPost]
    public async Task<Result> CreateMassLocation([FromBody] CreateMassLocationRequest request)
    {
        var massSchedules = request.MassSchedules?.Select(ms => new MassScheduleInput(ms.Day, ms.MassTimes)).ToList() ?? [];

        return await _sender.Send(new CreateMassLocationCommand(
            request.Name, 
            request.Address, 
            request.Latitude,
            request.Longitude,
            request.IsHeadquarters, 
            massSchedules));
    }

    [HasPermission(ParishManagementPermissions.ReadMassLocation)]
    [HttpGet]
    public async Task<Result<List<MassLocationResponse>>> GetAll([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        return await _sender.Send(new GetMassLocationsQuery(pageIndex, pageSize));
    }

    [HasPermission(ParishManagementPermissions.ReadMassLocation)]
    [HttpGet("{id}")]
    public async Task<Result<MassLocationByIdResponse>> GetById(Guid id)
    {
        return await _sender.Send(new GetMassLocationByIdQuery(id));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpPut("{id}")]
    public async Task<Result> UpdateMassLocation(Guid id, [FromBody] UpdateMassLocationRequest request)
    {
        return await _sender.Send(new UpdateMassLocationCommand(
            id, 
            request.Name, 
            request.Address,
            request.Latitude,
            request.Longitude,
            request.IsHeadquarters));
    }

    [HasPermission(ParishManagementPermissions.DeleteMassLocation)]
    [HttpDelete("{id}")]
    public async Task<Result> DeleteMassLocation(Guid id)
    {
        return await _sender.Send(new DeleteMassLocationCommand(id));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpPost("{id}/schedules")]
    public async Task<Result> AddScheduleToLocation(Guid id, [FromBody] AddScheduleToLocationRequest request)
    {
        return await _sender.Send(new AddScheduleToLocationCommand(id, request.Day, request.MassTimes));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpPut("{id}/schedules/{scheduleId}")]
    public async Task<Result> UpdateSchedule(Guid id, Guid scheduleId, [FromBody] UpdateScheduleRequest request)
    {
        return await _sender.Send(new UpdateScheduleCommand(id, scheduleId, request.Day));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpDelete("{id}/schedules/{scheduleId}")]
    public async Task<Result> RemoveScheduleFromLocation(Guid id, Guid scheduleId)
    {
        return await _sender.Send(new RemoveScheduleFromLocationCommand(id, scheduleId));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpPost("{id}/schedules/{scheduleId}/times")]
    public async Task<Result> AddTimeToSchedule(Guid id, Guid scheduleId, [FromBody] AddTimeToScheduleRequest request)
    {
        return await _sender.Send(new AddTimeToScheduleCommand(id, scheduleId, request.Time));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpPut("{id}/schedules/{scheduleId}/times/{timeId}")]
    public async Task<Result> UpdateTime(Guid id, Guid scheduleId, Guid timeId, [FromBody] UpdateTimeRequest request)
    {
        return await _sender.Send(new UpdateMassTimeCommand(id, scheduleId, timeId, request.Time));
    }

    [HasPermission(ParishManagementPermissions.UpdateMassLocation)]
    [HttpDelete("{id}/schedules/{scheduleId}/times/{timeId}")]
    public async Task<Result> RemoveTimeFromSchedule(Guid id, Guid scheduleId, Guid timeId)
    {
        return await _sender.Send(new RemoveTimeFromScheduleCommand(id, scheduleId, timeId));
    }
}
