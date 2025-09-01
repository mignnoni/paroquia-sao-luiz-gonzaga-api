using Ardalis.Result;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

namespace Modules.ParishManagement.Domain.NewsFolder;

public class News : Entity<NewsId>
{
    private News(NewsId id, string title, string content, bool highlight, DateTime? highlightUntil, string summary) : base(id)
    {
        Title = title;
        Content = content;
        Highlight = highlight;
        HighlightUntil = highlightUntil;
        Summary = summary;
    }

    // EF Core
    private News() { }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Summary { get; private set; }
    public bool Highlight { get; private set; }
    public DateTime? HighlightUntil { get; private set; }

    private readonly List<NewsFile> _files = [];
    public IReadOnlyCollection<NewsFile> Files => _files.AsReadOnly();

    public static Result<News> Create(NewsId id, string title, string content, bool highlight, DateTime? highlightUntil, string summary)
    {
        var validationResult = Validate(title, content, summary, highlight, highlightUntil);

        if (!validationResult.IsSuccess)
            return validationResult;

        var News = new News(id, title, content, highlight, highlightUntil, summary);

        return Result.Success(News);
    }

    public Result Update(string title, string content, bool highlight, DateTime? highlightUntil, string summary, List<Guid>? filesToRemove)
    {
        var validationResult = Validate(title, content, summary, highlight, highlightUntil);

        if (!validationResult.IsSuccess)
            return validationResult;

        if (filesToRemove is not null && filesToRemove.Count > 0)
        {
            var removeFilesResult = RemoveFiles(filesToRemove);

            if (!removeFilesResult.IsSuccess)
                return removeFilesResult;
        }

        Title = title;
        Content = content;
        Highlight = highlight;
        HighlightUntil = highlightUntil;
        Summary = summary;

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Unhighlight()
    {
        Highlight = false;
        HighlightUntil = null;

        UpdatedAt = DateTime.UtcNow;
    }

    public void AddFiles(List<UploadInfo> files)
    {
        foreach (var file in files)
        {
            var newsFile = NewsFile.Create(Guid.NewGuid(), Id, file);
            this.AddFile(newsFile);
        }
    }

    private void AddFile(NewsFile file)
    {
        _files.Add(file);
    }

    private Result RemoveFiles(List<Guid> filesToRemove)
    {
        foreach (var fileId in filesToRemove)
        {
            var file = _files.FirstOrDefault(f => f.Id == fileId);

            if (file is null)
                return Result.Error($"Arquivo {fileId} não encontrado");

            _files.Remove(file);
        }

        return Result.Success();
    }

    private static Result Validate(string title, string content, string summary, bool highlight, DateTime? highlightUntil)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Error("O título é obrigatório");

        if (string.IsNullOrWhiteSpace(content))
            return Result.Error("O conteúdo é obrigatório");

        if (string.IsNullOrWhiteSpace(summary))
            return Result.Error("O resumo é obrigatório");

        if (highlight)
        {
            if (!highlightUntil.HasValue)
                return Result.Error("O conteúdo destacado deve ter uma data limite");

            if (highlightUntil < DateTime.UtcNow)
                return Result.Error("A data limite deve ser maior que a data atual");
        }

        return Result.Success();
    }
}
