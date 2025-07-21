using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SecretariaApi.Util
{
    public static class SenhaHelper
    {
        public static bool ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            return regex.IsMatch(senha);
        }
        public static string GerarSalt(int tamanho = 16)
        {
            byte[] salt = new byte[tamanho];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GerarHash(string senha, string salt, int iteracoes = 10000)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, iteracoes, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); 
                return Convert.ToBase64String(hash);
            }
        }

        public static string CriptografarSenha(string senha)
        {
            var salt = GerarSalt();
            var hash = GerarHash(senha, salt);

            return $"{salt}:{hash}";
        }
        public static bool VerificarSenha(string senhaDigitada, string senhaArmazenada)
        {
            var partes = senhaArmazenada.Split(':');
            if (partes.Length != 2)
                return false;

            var salt = partes[0];
            var hashArmazenado = partes[1];

            var hashDigitado = GerarHash(senhaDigitada, salt);

            return hashArmazenado == hashDigitado;
        }
        public static bool VerificarSenhaLogin(string senhaFornecida, string senhaArmazenada)
        {
            var partes = senhaArmazenada.Split(':');
            if (partes.Length != 2)
            {
                throw new Exception("Senha armazenada inválida.");
            }

            var salt = partes[0];  
            var hashArmazenado = partes[1]; 

            var hashFornecido = GerarHash(senhaFornecida, salt);

            return hashFornecido == hashArmazenado;
        }
    }
}
