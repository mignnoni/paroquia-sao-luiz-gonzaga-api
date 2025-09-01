using Ardalis.Result;
using BuildingBlocks.Application;
using BuildingBlocks.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.NewsFolder;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.NewsFolder.UpdateNews;

public record UpdateNewsCommand(
    Guid Id,
    string Title,
    string Content,
    bool Highlight,
    DateTime? HighlightUntil,
    string Summary,
    List<FileRequest> FilesToAdd,
    List<Guid> FilesToRemove) : ICommand;

internal class UpdateNewsCommandHandler(
    INewsRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<UpdateNewsCommandHandler> _logger) : ICommandHandler<UpdateNewsCommand>
{
    private const int MAX_TOTAL_FILES = 10;

    public async Task<Result> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        var spec = new NewsByIdSpec(new NewsId(request.Id));

        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Error("Notícia não encontrada");

        if ((request.FilesToAdd.Count + news.Files.Count - request.FilesToRemove.Count) > MAX_TOTAL_FILES)
            return Result.Error("Só é possível ter no máximo 10 arquivos");

        List<string> filesToRemoveFromS3 = news.Files
            .Where(f => request.FilesToRemove.Contains(f.Id))
            .Select(f => f.UploadInfo.FileName)
            .ToList() ?? [];

        if (!news.Highlight && request.Highlight)
        {
            var highlightedNewsSpec = new HighlightedNewsSpec();

            var highlightedNews = await _repository.FirstOrDefaultAsync(highlightedNewsSpec, cancellationToken);

            highlightedNews?.Unhighlight();
        }

        var result = news.Update(request.Title, request.Content, request.Highlight, request.HighlightUntil, request.Summary, request.FilesToRemove);

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

            news.AddFiles(filesToAdd);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Notícia atualizada com sucesso: {NewsId}", news.Id);
        }
        catch (Exception ex)
        {
            await DeleteFilesAsync([.. filesToAdd.Select(f => f.FileName)], cancellationToken);
            _logger.LogError(ex, "Erro ao atualizar notícia");
            return Result.Error("Erro ao atualizar notícia");
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
