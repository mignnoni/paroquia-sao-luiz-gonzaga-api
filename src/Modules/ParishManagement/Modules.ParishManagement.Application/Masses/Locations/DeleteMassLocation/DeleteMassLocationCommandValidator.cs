using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Locations.DeleteMassLocation;

public class DeleteMassLocationCommandValidator : AbstractValidator<DeleteMassLocationCommand>
{
    public DeleteMassLocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID da localização de missas é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("ID da localização de missas não pode ser vazio");
    }
} 