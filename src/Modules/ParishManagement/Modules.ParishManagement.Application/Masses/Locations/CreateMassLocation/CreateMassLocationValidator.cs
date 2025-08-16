using System;
using FluentValidation;

namespace Modules.ParishManagement.Application.Masses.Locations.CreateMassLocation;

public class CreateMassLocationValidator : AbstractValidator<CreateMassLocationCommand>
{
    public CreateMassLocationValidator()
    {
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

        RuleForEach(c => c.MassSchedules)
            .ChildRules(schedule =>
            {
                schedule.RuleFor(s => s.Day)
                    .NotEmpty()
                    .WithMessage("É obrigatório informar o dia da programação de missas")
                    .MaximumLength(50)
                    .WithMessage("O dia da programação de missas não pode ter mais de 50 caracteres");
            });

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
