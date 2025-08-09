using FluentValidation;

namespace Modules.ParishManagement.Application.OtherSchedules.CreateOtherSchedule;

public class CreateOtherScheduleCommandValidator : AbstractValidator<CreateOtherScheduleCommand>
{
    public CreateOtherScheduleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Título é obrigatório")
            .MaximumLength(150)
            .WithMessage("O título não pode ter mais de 150 caracteres");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Conteúdo é obrigatório");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Tipo de programação é inválido");
    }
}