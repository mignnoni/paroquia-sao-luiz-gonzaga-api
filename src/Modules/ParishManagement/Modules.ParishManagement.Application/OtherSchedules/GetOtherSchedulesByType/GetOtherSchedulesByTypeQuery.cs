using Ardalis.Result;
using BuildingBlocks.Application;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.GetOtherScheduleById;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.GetOtherSchedulesByType;

public record GetOtherSchedulesByTypeQuery(ScheduleType Type) : IQuery<List<OtherScheduleResponse>>;

public record OtherScheduleResponse(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<OtherScheduleFileResponse> Files);

public class GetOtherSchedulesByTypeQueryHandler(
    IOtherScheduleRepository _repository,
    IS3Service _s3Service) : IQueryHandler<GetOtherSchedulesByTypeQuery, List<OtherScheduleResponse>>
{
    public async Task<Result<List<OtherScheduleResponse>>> Handle(GetOtherSchedulesByTypeQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(request.Type))
            return Result.Error("Tipo de programação inválido");

        var spec = new OtherSchedulesByTypeSpec(request.Type, true);

        var otherSchedules = await _repository.ListAsync(spec, cancellationToken) ?? [];

        var response = otherSchedules.Select(s => new OtherScheduleResponse(
            s.Id.Value,
            s.Title,
            s.Content,
            s.Type,
            s.CreatedAt,
            s.UpdatedAt,
            s.Files.Select(f => new OtherScheduleFileResponse(
                f.Id,
                f.UploadInfo.FileName,
                f.UploadInfo.ContentType,
                _s3Service.GetPublicUrl(f.UploadInfo.FileName))).ToList() ?? [])).ToList() ?? [];

        return Result.Success(response);
    }
}