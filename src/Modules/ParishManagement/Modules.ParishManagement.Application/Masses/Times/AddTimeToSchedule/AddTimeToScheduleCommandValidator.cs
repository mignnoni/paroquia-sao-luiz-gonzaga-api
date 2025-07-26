using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Times.AddTimeToSchedule;

public class AddTimeToScheduleCommandValidator : AbstractValidator<AddTimeToScheduleCommand>
{
    public AddTimeToScheduleCommandValidator()
    {
        RuleFor(x => x.MassLocationId)
            .NotEmpty()
            .WithMessage("ID da localização de missas é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da localização de missas não pode ser vazio");

        RuleFor(x => x.MassScheduleId)
            .NotEmpty()
            .WithMessage("ID da programação de missas é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da programação de missas não pode ser vazio");

        RuleFor(x => x.MassTime)
            .NotNull()
            .WithMessage("Horário da missa é obrigatório");
    }
} 