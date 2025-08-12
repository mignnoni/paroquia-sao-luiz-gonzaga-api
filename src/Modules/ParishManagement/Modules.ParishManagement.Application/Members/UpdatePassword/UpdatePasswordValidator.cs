using BuildingBlocks.Utilities;
using FluentValidation;


namespace Modules.ParishManagement.Application.Members.UpdatePassword;

internal class CreateMemberValidator : AbstractValidator<UpdatePasswordCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("A senha atual deve ser informada");

        RuleFor(x => x.CurrentPassword)
            .NotEqual(x => x.NewPassword)
            .WithMessage("A nova senha deve ser diferente da senha atual");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("A nova senha deve ser informada");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(s => s.NewPassword)
            .WithMessage("As senhas informadas devem ser iguais");
    }
}
