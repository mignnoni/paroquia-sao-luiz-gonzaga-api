using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Application.NewsFolder.Specifications;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Domain.NewsFolder;

namespace Modules.ParishManagement.Application.NewsFolder.DeleteNews;

public record DeleteNewsCommand(Guid Id) : ICommand;

internal class DeleteNewsCommandHandler(
    INewsRepository _repository,
    IUnitOfWork _unitOfWork,
    IS3Service _s3Service,
    ILogger<DeleteNewsCommandHandler> _logger) : ICommandHandler<DeleteNewsCommand>
{
    public async Task<Result> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var spec = new NewsByIdSpec(new NewsId(request.Id));

        var news = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (news is null)
            return Result.Error("Comunicado não encontrado");

        var files = news.Files.Select(f => f.UploadInfo.FileName).ToList() ?? [];

        _repository.Delete(news);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Comunicado removido com sucesso: {NewsId}",
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