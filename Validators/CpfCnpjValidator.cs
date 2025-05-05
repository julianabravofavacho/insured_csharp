using System.Linq;

namespace WebApi_Coris.Validators
{

    public static class CpfCnpjValidator
    {
        public static bool IsValid(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var digits = new string(value.Where(char.IsDigit).ToArray());
            if (digits.Length == 11)
                return IsCpf(digits);
            if (digits.Length == 14)
                return IsCnpj(digits);
            return false;
        }

        private static bool IsCpf(string cpf)
        {
            // Rejeita sequências repetidas
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Primeiro dígito
            var firstSum = 0;
            for (int i = 0; i < 9; i++)
                firstSum += (cpf[i] - '0') * (10 - i);
            var firstRemainder = firstSum % 11;
            var firstCheck = (firstRemainder < 2) ? 0 : 11 - firstRemainder;
            if (firstCheck != cpf[9] - '0')
                return false;

            // Segundo dígito
            var secondSum = 0;
            for (int i = 0; i < 10; i++)
                secondSum += (cpf[i] - '0') * (11 - i);
            var secondRemainder = secondSum % 11;
            var secondCheck = (secondRemainder < 2) ? 0 : 11 - secondRemainder;
            if (secondCheck != cpf[10] - '0')
                return false;

            return true;
        }

        private static bool IsCnpj(string cnpj)
        {
            // Rejeita sequências repetidas
            if (cnpj.All(c => c == cnpj[0]))
                return false;

            // Pesos para o primeiro dígito
            int[] firstWeights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int firstSum = 0;
            for (int i = 0; i < 12; i++)
                firstSum += (cnpj[i] - '0') * firstWeights[i];
            var firstRemainder = firstSum % 11;
            var firstCheck = (firstRemainder < 2) ? 0 : 11 - firstRemainder;
            if (firstCheck != cnpj[12] - '0')
                return false;

            // Pesos para o segundo dígito
            int[] secondWeights = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int secondSum = 0;
            for (int i = 0; i < 13; i++)
                secondSum += (cnpj[i] - '0') * secondWeights[i];
            var secondRemainder = secondSum % 11;
            var secondCheck = (secondRemainder < 2) ? 0 : 11 - secondRemainder;
            if (secondCheck != cnpj[13] - '0')
                return false;

            return true;
        }
    }

}
