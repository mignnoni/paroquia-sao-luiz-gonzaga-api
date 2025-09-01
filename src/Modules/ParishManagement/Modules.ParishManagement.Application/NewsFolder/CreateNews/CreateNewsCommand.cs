using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.CreateNews;

public record CreateNewsCommand(
    string Title,
    string Content,
    string Summary,
    bool Highlight,
    DateTime? HighlightUntil,
    List<FileRequest> Files) : ICommand;

internal class CreateNewsCommandHandler(
    INewsRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<CreateNewsCommandHandler> _logger) : ICommandHandler<CreateNewsCommand>
{
    private const int MAX_FILES = 5;

    public async Task<Result> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        if (request.Files.Count > MAX_FILES)
            return Result.Error("Só é possível adicionar até 5 arquivos	por vez");

        if (request.Highlight)
        {
            var spec = new HighlightedNewsSpec();

            var highlightedNews = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

            highlightedNews?.Unhighlight();
        }

        var result = News.Create(
            new NewsId(Guid.NewGuid()),
            request.Title,
            request.Content,
            request.Highlight,
            request.HighlightUntil,
            request.Summary);

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

            _logger.LogInformation("Comunicado criado com sucesso: {NewsId}", otherSchedule.Id);
        }
        catch (Exception ex)
        {
            await DeleteFilesAsync(files, cancellationToken);

            _logger.LogError(ex, "Erro ao criar comunicado");

            return Result.Error("Erro ao criar comunicado");
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