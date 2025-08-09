using FluentValidation;

namespace Modules.ParishManagement.Application.OtherSchedules.UpdateOtherSchedule;

public class UpdateOtherScheduleCommandValidator : AbstractValidator<UpdateOtherScheduleCommand>
{
    public UpdateOtherScheduleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID da programação é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da programação não pode ser vazio");

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

        RuleFor(x => x.FilesToAdd)
            .Must(x => x.Count <= 5)
            .WithMessage("Só é possível adicionar até 5 arquivos por vez");
    }
}