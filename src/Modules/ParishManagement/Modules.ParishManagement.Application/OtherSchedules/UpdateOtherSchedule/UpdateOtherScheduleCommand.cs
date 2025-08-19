using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.OtherSchedules.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.UpdateOtherSchedule;

public record UpdateOtherScheduleCommand(
    Guid Id,
    string Title,
    string Content,
    ScheduleType Type,
    List<FileRequest> FilesToAdd,
    List<Guid> FilesToRemove) : ICommand;

internal class UpdateOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<UpdateOtherScheduleCommandHandler> _logger) : ICommandHandler<UpdateOtherScheduleCommand>
{
    private const int MAX_TOTAL_FILES = 10;

    public async Task<Result> Handle(UpdateOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        var spec = new OtherScheduleByIdSpec(new OtherScheduleId(request.Id));

        var otherSchedule = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (otherSchedule is null)
            return Result.Error("Programação não encontrada");

        if ((request.FilesToAdd.Count + otherSchedule.Files.Count - request.FilesToRemove.Count) > MAX_TOTAL_FILES)
            return Result.Error("Só é possível ter no máximo 10 arquivos");

        List<string> filesToRemoveFromS3 = otherSchedule.Files
            .Where(f => request.FilesToRemove.Contains(f.Id))
            .Select(f => f.UploadInfo.FileName)
            .ToList() ?? [];

        var result = otherSchedule.Update(request.Title, request.Content, request.Type, request.FilesToRemove);

        if (!result.IsSuccess)
            return result;

        List<UploadInfo> filesToAdd = [];

        try
        {
            foreach (var file in request.FilesToAdd)
            {
                string uploadedName = await _s3Service.UploadFileAsync(
                    file.FileStream,
                    file.Name,
                    file.ContentType,
                    file.Extension,
                    cancellationToken);

                var uploadInfo = UploadInfo.Create(file.Name, uploadedName, file.ContentType);

                filesToAdd.Add(uploadInfo);
            }

            otherSchedule.AddFiles(filesToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Programação atualizada com sucesso: {OtherScheduleId}", otherSchedule.Id);
        }
        catch (Exception ex)
        {
            await DeleteFilesAsync([.. filesToAdd.Select(f => f.FileName)], cancellationToken);
            _logger.LogError(ex, "Erro ao atualizar programação");
            return Result.Error("Erro ao atualizar programação");
        }
        finally
        {
            foreach (var file in request.FilesToAdd)
            {
                file.FileStream?.Dispose();
            }
        }

        if (filesToRemoveFromS3.Count > 0)
        {
            await DeleteFilesAsync(filesToRemoveFromS3, cancellationToken);
        }

        return Result.Success();
    }

    private async Task DeleteFilesAsync(List<string> files, CancellationToken cancellationToken)
    {
        if (files.Count == 0)
            return;

        try
        {
            await _s3Service.DeleteFilesAsync(files, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover os seguintes arquivos: {Files}", string.Join(", ", files));
        }
    }
}
