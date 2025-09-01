using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocations;
using Modules.ParishManagement.Application.Masses.Locations.Public.GetMassLocationHeadquarters;
using Modules.ParishManagement.Application.NewsFolder.Public.GetNews;
using Modules.ParishManagement.Application.NewsFolder.Public.GetNewsById;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherScheduleById;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetOtherSchedules;
using Modules.ParishManagement.Domain.OtherSchedules;
using Modules.ParishManagement.Application.OtherSchedules.Public.GetFirstOtherScheduleByType;
using Modules.ParishManagement.Application.NewsFolder.Public.GetHighlightedNews;

namespace ParoquiaSLG.API.Modules.Public;

[Route("api/v1/[controller]")]
[ApiController]
[TranslateResultToActionResult]
public class PublicController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("mass-locations")]
    public async Task<Result<List<MassLocationPublicResponse>>> GetAll([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 25)
    {
        return await _sender.Send(new GetMassLocationsQuery(pageIndex, pageSize));
    }

    [HttpGet("mass-locations/headquarters")]
    public async Task<Result<MassLocationPublicResponse>> GetHeadquarters()
    {
        return await _sender.Send(new GetMassLocationHeadquartersQuery());
    }

    [HttpGet("news")]
    public async Task<Result<List<NewsResponse>>> GetNews([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        return await _sender.Send(new GetNewsQuery(pageIndex, pageSize));
    }

    [HttpGet("news/{id}")]
    public async Task<Result<NewsByIdResponse>> GetNewsById(Guid id)
    {
        return await _sender.Send(new GetNewsByIdQuery(id));
    }

    [HttpGet("news/highlighted")]
    public async Task<Result<NewsByIdResponse?>> GetHighlightedNews()
    {
        return await _sender.Send(new GetHighlightedNewsQuery());
    }

    [HttpGet("other-schedules")]
    public async Task<Result<List<OtherScheduleResponse>>> GetOtherSchedules([FromQuery] ScheduleType type)
    {
        return await _sender.Send(new GetOtherSchedulesQuery(type));
    }

    [HttpGet("other-schedules/{id}")]
    public async Task<Result<OtherScheduleResponse>> GetOtherScheduleById(Guid id)
    {
        return await _sender.Send(new GetOtherScheduleByIdQuery(id));
    }

    [HttpGet("other-schedules/first-by-type")]
    public async Task<Result<OtherScheduleResponse>> GetFirstOtherScheduleByType([FromQuery] ScheduleType type)
    {
        return await _sender.Send(new GetFirstOtherScheduleByTypeQuery(type));
    }
}