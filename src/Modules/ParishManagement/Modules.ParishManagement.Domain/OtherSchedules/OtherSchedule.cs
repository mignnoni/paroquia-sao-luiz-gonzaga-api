using Ardalis.Result;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

namespace Modules.ParishManagement.Domain.OtherSchedules;

public class OtherSchedule : Entity<OtherScheduleId>
{
    private OtherSchedule(OtherScheduleId id, string title, string content, ScheduleType type) : base(id)
    {
        Title = title;
        Content = content;
        Type = type;
    }

    // EF Core
    private OtherSchedule() { }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public ScheduleType Type { get; private set; }
    private readonly List<OtherScheduleFile> _files = [];
    public IReadOnlyCollection<OtherScheduleFile> Files => _files.AsReadOnly();

    public static Result<OtherSchedule> Create(OtherScheduleId id, string title, string content, ScheduleType type)
    {
        var validationResult = Validate(title, content, type);

        if (!validationResult.IsSuccess)
            return validationResult;

        var otherSchedule = new OtherSchedule(id, title, content, type);

        return Result.Success(otherSchedule);
    }

    public Result Update(string title, string content, ScheduleType type, List<Guid>? filesToRemove)
    {
        var validationResult = Validate(title, content, type);

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
        Type = type;

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void AddFiles(List<UploadInfo> files)
    {
        foreach (var file in files)
        {
            var otherScheduleFile = OtherScheduleFile.Create(Guid.NewGuid(), Id, file);
            this.AddFile(otherScheduleFile);
        }
    }

    private void AddFile(OtherScheduleFile file)
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

    private static Result Validate(string title, string content, ScheduleType type)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Error("O título é obrigatório");

        if (string.IsNullOrWhiteSpace(content))
            return Result.Error("O conteúdo é obrigatório");

        if (!Enum.IsDefined(type))
            return Result.Error("O tipo de programação é inválido");

        return Result.Success();
    }
}
