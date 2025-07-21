using SecretariaApi.Dto;
using SecretariaApi.IRepository;
using SecretariaApi.IService;
using SecretariaApi.Util;
using SecretariaApi.Util.Enum;

namespace SecretariaApi.Service
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IUsuarioService _usuarioService;

        public AlunoService(IAlunoRepository alunoRepository, IUsuarioService usuarioService)
        {
            _alunoRepository = alunoRepository;
            _usuarioService = usuarioService;        }

        public async Task<AlunoUsuarioDto?> BuscarPorIdAsync(int id)
        {
            return await _alunoRepository.BuscarPorIdAsync(id);
        }
        public async Task<IEnumerable<AlunoUsuarioDto>> BuscarPorNomeAsync(string nomeAluno, int pageNumber = 1, int pageSize = 10)
        {
            return await _alunoRepository.BuscarPorNomeAsync(nomeAluno, pageNumber, pageSize);
        }

        public async Task<Result> CadastrarAsync(AlunoUsuarioDto alunoDto)
        {
            try
            {
                var resultadoValidacao = ValidarAluno.Validar(alunoDto);
                if (!resultadoValidacao.Success)
                    return resultadoValidacao; 

                string senhaHash = SenhaHelper.CriptografarSenha(alunoDto.Senha);

                var alunoCadastrar = new AlunoUsuarioDto()
                {
                    Nome = alunoDto.Nome,
                    Cpf = alunoDto.Cpf,
                    Email = alunoDto.Email,
                    Senha = senhaHash,
                    DtNascimento = alunoDto.DtNascimento,
                    TipoUsuario = (int)TipoUsuario.Aluno,
                    IdUsuarioInclusao = Convert.ToInt16( _usuarioService.GetUsuarioId())
                };
                await _alunoRepository.CadastrarAsync(alunoCadastrar);
                return new Result { Success = true };

            }
            catch (Exception)
            {
                return new Result { Success = false, ErrorMessage = "Erro ao realizar cadastro." };
            }
        }

        public async Task<Result> AtualizarAsync(AlunoUsuarioDto alunoDto)
        {
            var resultadoValidacao = ValidarAluno.Validar(alunoDto);
            if (!resultadoValidacao.Success)
                return resultadoValidacao;
            try
            {
                var alunoExistente = await _alunoRepository.BuscarPorIdAsync(alunoDto.IdAluno);
                if (alunoExistente == null)
                    return new Result { Success = false, ErrorMessage = "Aluno não encontrado." };

                string senhaHash = string.IsNullOrEmpty(alunoDto.Senha)
                    ? alunoExistente.Senha 
                    : SenhaHelper.CriptografarSenha(alunoDto.Senha); 

                alunoExistente.Nome = alunoDto.Nome;
                alunoExistente.Cpf = alunoDto.Cpf;
                alunoExistente.Email = alunoDto.Email;
                alunoExistente.Senha = senhaHash;
                alunoExistente.DtNascimento = alunoDto.DtNascimento;
                alunoExistente.IdUsuarioAlteracao = Convert.ToInt16(_usuarioService.GetUsuarioId());


                await _alunoRepository.AtualizarAsync(alunoExistente);

                return new Result { Success = true }; 
            }
            catch (Exception)
            {
                return new Result { Success = false, ErrorMessage = "Erro ao realizar atualização." };
            }
        }

        public async Task<Result> RemoverAsync(AlunoUsuarioDto aluno)
        {
            try
            {
                var alunoExistente = await _alunoRepository.BuscarPorIdAsync(aluno.IdAluno);
                if (alunoExistente == null)
                    return new Result { Success = false, ErrorMessage = "Aluno não encontrado." };

                var resultado = await _alunoRepository.RemoverAsync(aluno);

                if (!resultado)
                {
                    return new Result { Success = false, ErrorMessage = "Falha ao remover o aluno." };
                }

                return new Result { Success = true };
            }
            catch (Exception)
            {

                return new Result { Success = false, ErrorMessage = "Erro ao realizar remoção." };
            }
        }
    }
}
