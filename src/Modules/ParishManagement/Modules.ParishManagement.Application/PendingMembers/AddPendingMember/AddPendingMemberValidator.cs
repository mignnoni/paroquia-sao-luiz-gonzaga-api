using System;
using FluentValidation;

namespace Modules.ParishManagement.Application.PendingMembers.AddPendingMember;

public class AddPendingMemberValidator : AbstractValidator<AddPendingMemberCommand>
{
    public AddPendingMemberValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("O nome completo é obrigatório");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("O e-mail informado não é válido");
    }
}
