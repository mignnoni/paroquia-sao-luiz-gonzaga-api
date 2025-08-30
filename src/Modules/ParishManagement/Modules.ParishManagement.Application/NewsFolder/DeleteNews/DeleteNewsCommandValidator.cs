using FluentValidation;

namespace Modules.ParishManagement.Application.NewsFolder.DeleteNews;

public class DeleteNewsCommandValidator : AbstractValidator<DeleteNewsCommand>
{
    public DeleteNewsCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID da programação é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da programação não pode ser vazio");
    }
}