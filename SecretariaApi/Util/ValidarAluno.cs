using SecretariaApi.Dto;

namespace SecretariaApi.Util
{
    public class ValidarAluno
    {
        public static Result Validar(AlunoUsuarioDto alunoDto)
        {
            if (!Helper.ValidarCpf(alunoDto.Cpf))
                return new Result { Success = false, ErrorMessage = "Cpf Invalido. Digite um Cpf Valido" };

            if (string.IsNullOrEmpty(alunoDto.Nome) || alunoDto.Nome.Length < 3)
                return new Result { Success = false, ErrorMessage = "Nome precisa ter mais que 3 caracteres" };

            if (!Helper.ValidarEmail(alunoDto.Email))
                return new Result { Success = false, ErrorMessage = "E-mail Invalido, por favor digitar um E-mail valido." };

            if (!SenhaHelper.ValidarSenha(alunoDto.Senha))
                return new Result { Success = false, ErrorMessage = "A senha é fraca. Use pelo menos 8 caracteres com letras maiúsculas, minúsculas, números e símbolos." };

            if (!Helper.ValidarDataNascimento(alunoDto.DtNascimento))
                return new Result { Success = false, ErrorMessage = "Data de Nascimento Invalida" };

            return new Result { Success = true };
        }
    }
}
