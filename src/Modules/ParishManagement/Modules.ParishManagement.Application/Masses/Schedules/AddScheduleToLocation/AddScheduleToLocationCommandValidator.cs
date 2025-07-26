using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Schedules.AddScheduleToLocation;

public class AddScheduleToLocationCommandValidator : AbstractValidator<AddScheduleToLocationCommand>
{
    public AddScheduleToLocationCommandValidator()
    {
        RuleFor(x => x.MassLocationId)
            .NotEmpty()
            .WithMessage("ID da localização de missas é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da localização de missas não pode ser vazio");

        RuleFor(x => x.Day)
            .NotEmpty()
            .WithMessage("Dia da programação é obrigatório")
            .MaximumLength(50)
            .WithMessage("O dia da programação não pode ter mais de 50 caracteres");
    }
}