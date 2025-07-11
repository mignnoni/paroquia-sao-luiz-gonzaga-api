namespace BuildingBlocks.Utilities;

public static class StringExtensions
{
    public static bool IsValidCpf(this string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        // Remove qualquer ponto ou traço do CPF
        cpf = cpf.Replace(".", "").Replace("-", "");

        // Verifica se tem 11 dígitos e se não são todos iguais
        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        // Validação dos dígitos verificadores
        var cpfDigits = cpf.Select(c => int.Parse(c.ToString())).ToArray();

        // Cálculo do primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += cpfDigits[i] * (10 - i);

        int firstVerifier = (sum * 10) % 11;
        if (firstVerifier == 10) firstVerifier = 0;

        if (cpfDigits[9] != firstVerifier)
            return false;

        // Cálculo do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += cpfDigits[i] * (11 - i);

        int secondVerifier = (sum * 10) % 11;
        if (secondVerifier == 10) secondVerifier = 0;

        return cpfDigits[10] == secondVerifier;
    }

    public static bool IsValidCnpj(this string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        // Remove non-digit characters
        cnpj = new string([.. cnpj.Where(char.IsDigit)]);

        // CNPJ must have 14 digits
        if (cnpj.Length != 14)
            return false;

        // Check for obvious invalid patterns
        if (cnpj.Distinct().Count() == 1)
            return false;

        // Calculate first verification digit
        int sum = 0;
        int[] weight1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        for (int i = 0; i < 12; i++)
            sum += (cnpj[i] - '0') * weight1[i];

        int digit1 = sum % 11;
        digit1 = digit1 < 2 ? 0 : 11 - digit1;

        // Check first verification digit
        if (digit1 != (cnpj[12] - '0'))
            return false;

        // Calculate second verification digit
        sum = 0;
        int[] weight2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        for (int i = 0; i < 13; i++)
            sum += (cnpj[i] - '0') * weight2[i];

        int digit2 = sum % 11;
        digit2 = digit2 < 2 ? 0 : 11 - digit2;

        // Check second verification digit
        return digit2 == (cnpj[13] - '0');
    }
}
