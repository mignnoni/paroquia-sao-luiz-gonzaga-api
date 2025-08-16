using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Locations.UpdateMassLocation;

public class UpdateMassLocationValidator : AbstractValidator<UpdateMassLocationCommand>
{
    public UpdateMassLocationValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Id da localização é obrigatório")
            .NotEqual(Guid.Empty)
            .WithMessage("Id da localização é inválido");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Nome da localização é obrigatório")
            .MaximumLength(80)
            .WithMessage("O nome da localização não pode ter mais de 80 caracteres");

        RuleFor(c => c.Address)
            .NotEmpty()
            .WithMessage("Endereço da localização é obrigatório")
            .MaximumLength(200)
            .WithMessage("O endereço da localização não pode ter mais de 200 caracteres");

        RuleFor(c => c.Latitude)
            .NotEmpty()
            .WithMessage("Endereço da localização é obrigatório")
            .ExclusiveBetween(-90, 90)
            .WithMessage("Informe um endereço válido");

        RuleFor(c => c.Longitude)
            .NotEmpty()
            .WithMessage("Endereço da localização é obrigatório")
            .ExclusiveBetween(-180, 180)
            .WithMessage("Informe um endereço válido");
    }
}
