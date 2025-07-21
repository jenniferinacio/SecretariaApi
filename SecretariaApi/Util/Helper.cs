using System.ComponentModel;
using System.Text.RegularExpressions;
using SecretariaApi.Util.Enum;

namespace SecretariaApi.Util
{
    public static class Helper
    {
        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if ((cpf[9] - '0') != digito1)
                return false;

            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            if ((cpf[10] - '0') != digito2)
                return false;

            return true;
        }
        public static bool ValidarEmail(string email)
        {
            email = email.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
                return false;

            return true;
        }
        public static bool ValidarDataNascimento(DateTime dataNascimento, int idadeMinima = 18, int idadeMaxima = 150)
        {
            var hoje = DateTime.Today;

            if (dataNascimento > hoje)
                return false;

            int idade = hoje.Year - dataNascimento.Year;

            if (dataNascimento > hoje.AddYears(-idade))
                idade--;

            if (idade < idadeMinima || idade > idadeMaxima)
                return false;

            return true;
        }

        public static string GetEnumDescription(TipoUsuario value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString(); 
        }

    }
}
