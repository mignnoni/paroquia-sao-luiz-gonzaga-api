using FluentValidation;

namespace Modules.ParishManagement.Application.NewsFolder.UpdateNews;

public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsCommandValidator()
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

        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("Resumo é obrigatório")
            .MaximumLength(200)
            .WithMessage("O resumo não pode ter mais de 200 caracteres");

        RuleFor(x => x.FilesToAdd)
            .Must(x => x.Count <= 5)
            .WithMessage("Só é possível adicionar até 5 arquivos por vez");
    }
}