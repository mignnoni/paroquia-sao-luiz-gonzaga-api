using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Times.RemoveTimeFromSchedule;

public class RemoveTimeFromScheduleCommandValidator : AbstractValidator<RemoveTimeFromScheduleCommand>
{
    public RemoveTimeFromScheduleCommandValidator()
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

        RuleFor(x => x.MassTimeId)
            .NotEmpty()
            .WithMessage("ID do horário de missa é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID do horário de missa não pode ser vazio");
    }
} 