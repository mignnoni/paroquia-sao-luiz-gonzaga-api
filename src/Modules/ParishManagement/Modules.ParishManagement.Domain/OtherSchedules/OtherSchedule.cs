using Ardalis.Result;
using BuildingBlocks.Domain;

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

    public static Result<OtherSchedule> Create(OtherScheduleId id, string title, string content, ScheduleType type)
    {
        var validationResult = Validate(title, content, type);

        if (!validationResult.IsSuccess)
            return validationResult;

        var otherSchedule = new OtherSchedule(id, title, content, type);

        return Result.Success(otherSchedule);
    }

    public Result Update(string title, string content, ScheduleType type)
    {
        var validationResult = Validate(title, content, type);

        if (!validationResult.IsSuccess)
            return validationResult;

        Title = title;
        Content = content;
        Type = type;

        UpdatedAt = DateTime.UtcNow;

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
