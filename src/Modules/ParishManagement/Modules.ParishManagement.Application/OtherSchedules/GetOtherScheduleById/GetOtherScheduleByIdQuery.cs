using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.GetOtherScheduleById;

public record GetOtherScheduleByIdQuery(Guid Id) : IQuery<OtherScheduleByIdResponse>;

public record OtherScheduleByIdResponse(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<OtherScheduleFileResponse> Files);

public record OtherScheduleFileResponse(
    Guid Id,
    string Name,
    string ContentType,
    string Url);

public class GetOtherScheduleByIdQueryHandler(
    IOtherScheduleRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetOtherScheduleByIdQuery, OtherScheduleByIdResponse>
{
    public async Task<Result<OtherScheduleByIdResponse>> Handle(GetOtherScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            return Result.Error("O ID da programação é obrigatório");

        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id), true);
        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        var files = otherSchedule.Files.Select(f => new OtherScheduleFileResponse(
            f.Id,
            f.UploadInfo.FileName,
            f.UploadInfo.ContentType,
            _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList() ?? [];

        var response = new OtherScheduleByIdResponse(
            otherSchedule.Id.Value,
            otherSchedule.Title,
            otherSchedule.Content,
            otherSchedule.Type,
            otherSchedule.CreatedAt,
            otherSchedule.UpdatedAt,
            files);

        return Result.Success(response);
    }
}