using FluentValidation;

namespace Modules.ParishManagement.Application.OtherSchedules.DeleteOtherSchedule;

public class DeleteOtherScheduleCommandValidator : AbstractValidator<DeleteOtherScheduleCommand>
{
    public DeleteOtherScheduleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID da programação é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da programação não pode ser vazio");
    }
}