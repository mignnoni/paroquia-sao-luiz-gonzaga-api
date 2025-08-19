using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.OtherSchedules;

namespace Modules.ParishManagement.Application.OtherSchedules.CreateOtherSchedule;

public record CreateOtherScheduleCommand(
    string Title,
    string Content,
    ScheduleType Type,
    List<FileRequest> Files) : ICommand;

internal class CreateOtherScheduleCommandHandler(
    IOtherScheduleRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<CreateOtherScheduleCommandHandler> _logger) : ICommandHandler<CreateOtherScheduleCommand>
{
    private const int MAX_FILES = 5;

    public async Task<Result> Handle(CreateOtherScheduleCommand request, CancellationToken cancellationToken)
    {
        if (request.Files.Count > MAX_FILES)
            return Result.Error("Só é possível adicionar até 5 arquivos	por vez");

        var result = OtherSchedule.Create(
            new OtherScheduleId(Guid.NewGuid()),
            request.Title,
            request.Content,
            request.Type);

        if (!result.IsSuccess)
        {
            return Result.Error(result.Errors.First());
        }

        var otherSchedule = result.Value;

        List<UploadInfo> files = [];

        try
        {
            foreach (var file in request.Files)
            {
                string uploadedName = await _s3Service.UploadFileAsync(
                    file.FileStream,
                    file.Name,
                    file.ContentType,
                    file.Extension,
                    cancellationToken);

                var uploadInfo = UploadInfo.Create(file.Name, uploadedName, file.ContentType);

                files.Add(uploadInfo);
            }

            otherSchedule.AddFiles(files);

            _repository.Add(otherSchedule);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Programação criada com sucesso: {OtherScheduleId}", otherSchedule.Id);
        }
        catch (Exception ex)
        {
            await DeleteFilesAsync(files, cancellationToken);
            _logger.LogError(ex, "Erro ao criar programação");
            return Result.Error("Erro ao criar programação");
        }
        finally
        {
            foreach (var file in request.Files)
            {
                file.FileStream?.Dispose();
            }
        }

        return Result.Success();
    }

    private async Task DeleteFilesAsync(List<UploadInfo> files, CancellationToken cancellationToken)
    {
        if (files.Count == 0)
            return;

        try
        {
            await _s3Service.DeleteFilesAsync([.. files.Select(f => f.FileName)], cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover os seguintes arquivos: {Files}", string.Join(", ", files.Select(f => f.FileName)));
        }
    }
}