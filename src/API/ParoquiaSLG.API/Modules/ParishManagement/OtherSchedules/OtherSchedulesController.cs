using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BuildingBlocks.Application;
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
        List<FileRequest> files = [];

        if (request.Files is not null)
        {
            foreach (var file in request.Files)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result.Error($"O arquivo {file.FileName} excede o tamanho máximo permitido (10MB)");

                using var stream = file.OpenReadStream();
                using var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                files.Add(new FileRequest(file.FileName, file.ContentType, Path.GetExtension(file.FileName), memoryStream));
            }
        }

        return await _sender.Send(new CreateOtherScheduleCommand(request.Title, request.Content, request.Type, files));
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
        List<FileRequest> filesToAdd = [];

        if (request.FilesToAdd is not null)
        {
            foreach (var file in request.FilesToAdd)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result.Error($"O arquivo {file.FileName} excede o tamanho máximo permitido (10MB)");

                using var stream = file.OpenReadStream();
                using var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                filesToAdd.Add(new FileRequest(file.FileName, file.ContentType, Path.GetExtension(file.FileName), memoryStream));
            }
        }

        return await _sender.Send(new UpdateOtherScheduleCommand(id, request.Title, request.Content, request.Type, filesToAdd, request.FilesToRemove ?? []));
    }

    [HasPermission(ParishManagementPermissions.DeleteOtherSchedule)]
    [HttpDelete("{id}")]
    public async Task<Result> DeleteOtherSchedule(Guid id)
    {
        return await _sender.Send(new DeleteOtherScheduleCommand(id));
    }
}
