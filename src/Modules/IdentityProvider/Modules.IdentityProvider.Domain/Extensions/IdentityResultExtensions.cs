
using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Modules.IdentityProvider.Domain.Extensions;

public static class IdentityResultExtensions
{
    public static string? GetValidationErrorsString(this IdentityResult result)
    {
        if (result != null && result.Succeeded)
            return null;

        if (result is null)
            return "Erro ao utilizar o Asp.Net Identity";

        StringBuilder sb = new();

        sb.AppendJoin(" | ", result.Errors.Select(s => s.Description));

        return sb.ToString();
    }

    public static List<ValidationError>? GetValidationErrors(this IdentityResult? result)
    {
        if (result != null && result.Succeeded)
            return null;

        if (result == null)
        {
            return new List<ValidationError>()
            {
                new ValidationError
                {
                    Identifier = nameof(result),
                    ErrorCode = "412",
                    ErrorMessage = "Erro ao utilizar o Asp.Net Identity",
                    Severity = ValidationSeverity.Error
                }
            };
        }
        else
        {
            List<ValidationError> validationErrors = new();

            foreach (var error in result.Errors)
            {
                validationErrors.Add(new ValidationError(
                        identifier: nameof(error),
                        errorMessage: error.Description, 
                        errorCode: error.Code, 
                        severity: ValidationSeverity.Error
                    ));
            }

            return validationErrors;
        }
    }
}

