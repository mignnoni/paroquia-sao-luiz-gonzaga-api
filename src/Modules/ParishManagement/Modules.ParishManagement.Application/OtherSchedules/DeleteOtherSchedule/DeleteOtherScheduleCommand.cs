using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.DeleteOtherSchedule;

public record DeleteOtherScheduleCommand(Guid Id) : ICommand;

internal class DeleteOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<DeleteOtherScheduleCommandHandler> _logger) : ICommandHandler<DeleteOtherScheduleCommand>
{
    public async Task<Result> Handle(DeleteOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id));

        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        var files = otherSchedule.Files.Select(f => f.UploadInfo.FileName).ToList() ?? [];

        _repository.Delete(otherSchedule);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Programação removida com sucesso: {OtherScheduleId}",
            request.Id);

        if (files.Count > 0)
        {
            try
            {
                await _s3Service.DeleteFilesAsync(files, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover os seguintes arquivos da programação: {Files}", string.Join(", ", files));
            }
        }

        return Result.Success();
    }
}