using FluentValidation;

namespace Modules.ParishManagement.Application.Members.CreateMember;

internal class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.Name)
            .Must(s => s.Split(' ').Length > 1)
            .WithMessage("Deve ser informado o nome completo");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5)
            .WithMessage("O nome completo deve ter no mínimo 5 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha deve ser informada");

        RuleFor(x => x.ConfirmPassword)
            .Equal(s => s.Password)
            .WithMessage("As senhas informadas devem ser iguais");
    }
}
