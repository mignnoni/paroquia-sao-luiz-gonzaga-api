using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BuildingBlocks.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParoquiaSLG.API.Authorization;
using ParoquiaSLG.API.Modules.ParishManagement.NewsFolder.Contracts;
using Modules.ParishManagement.Application.NewsFolder.CreateNews;
using Modules.ParishManagement.Application.NewsFolder.GetNews;
using Modules.ParishManagement.Application.NewsFolder.GetNewsById;
using Modules.ParishManagement.Application.NewsFolder.UpdateNews;
using Modules.ParishManagement.Application.NewsFolder.DeleteNews;

namespace ParoquiaSLG.API.Modules.ParishManagement.NewsFolder;

[Route("[controller]")]
[ApiController]
[TranslateResultToActionResult]
public class NewsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HasPermission(ParishManagementPermissions.CreateOtherSchedule)]
    [HttpPost]
    public async Task<Result> CreateNews([FromForm] CreateNewsRequest request)
    {
        List<FileRequest> files = [];

        if (request.Files is not null)
        {
            foreach (var file in request.Files)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result.Error($"O arquivo {file.FileName} excede o tamanho máximo permitido (10MB)");

                using var stream = file.OpenReadStream();
                var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                files.Add(new FileRequest(file.FileName, file.ContentType, Path.GetExtension(file.FileName), memoryStream));
            }
        }

        return await _sender.Send(new CreateNewsCommand(request.Title, request.Content, request.Summary, request.Highlight, request.HighlightUntil, files));
    }

    [HasPermission(ParishManagementPermissions.ReadOtherSchedule)]
    [HttpGet]
    public async Task<Result<List<NewsResponse>>> GetAll(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10)
    {
        return await _sender.Send(new GetNewsQuery(pageIndex, pageSize));
    }

    [HasPermission(ParishManagementPermissions.ReadOtherSchedule)]
    [HttpGet("{id}")]
    public async Task<Result<NewsByIdResponse>> GetById(Guid id)
    {
        return await _sender.Send(new GetNewsByIdQuery(id));
    }

    [HasPermission(ParishManagementPermissions.UpdateOtherSchedule)]
    [HttpPut("{id}")]
    public async Task<Result> UpdateNews(Guid id, [FromForm] UpdateNewsRequest request)
    {
        List<FileRequest> filesToAdd = [];

        if (request.FilesToAdd is not null)
        {
            foreach (var file in request.FilesToAdd)
            {
                if (file.Length > 10 * 1024 * 1024)
                    return Result.Error($"O arquivo {file.FileName} excede o tamanho máximo permitido (10MB)");

                using var stream = file.OpenReadStream();
                var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                filesToAdd.Add(new FileRequest(file.FileName, file.ContentType, Path.GetExtension(file.FileName), memoryStream));
            }
        }

        return await _sender.Send(new UpdateNewsCommand(
            id,
            request.Title,
            request.Content,
            request.Highlight,
            request.HighlightUntil,
            request.Summary, filesToAdd,
            request.FilesToRemove ?? []));
    }

    [HasPermission(ParishManagementPermissions.DeleteOtherSchedule)]
    [HttpDelete("{id}")]
    public async Task<Result> DeleteNews(Guid id)
    {
        return await _sender.Send(new DeleteNewsCommand(id));
    }
}
