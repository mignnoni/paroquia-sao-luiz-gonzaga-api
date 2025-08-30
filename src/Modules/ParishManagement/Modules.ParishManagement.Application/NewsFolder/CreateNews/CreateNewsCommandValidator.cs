using FluentValidation;

namespace Modules.ParishManagement.Application.NewsFolder.CreateNews;

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
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
            .WithMessage("O resumo é obrigatório")
            .MaximumLength(200)
            .WithMessage("O resumo deve ter no máximo 200 caracteres");
    }
}