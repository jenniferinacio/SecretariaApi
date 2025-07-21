using SecretariaApi.Dto;
using SecretariaApi.IRepository;
using SecretariaApi.IService;
using SecretariaApi.Models;
using SecretariaApi.Util;

namespace SecretariaApi.Service
{
    public class TurmaService: ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IUsuarioService _usuarioService;

        public TurmaService(ITurmaRepository turmaRepository, IUsuarioService usuarioService)
        {
            _turmaRepository = turmaRepository;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<TurmaDto>> BuscarPorNomeAsync(string nomeAluno, int pageNumber = 1, int pageSize = 10)
        {
            return await _turmaRepository.BuscarPorNomeAsync(nomeAluno, pageNumber, pageSize);
        }
        public async Task<Result> CadastrarAsync(Turma turma)
        {
            try
            {
                if (string.IsNullOrEmpty(turma.Nome) || turma.Nome.Length < 3)
                    return new Result { Success = false, ErrorMessage = "Nome da Turma precisa ter mais que 3 caracteres" };

                turma.IdUsuarioInclusao = Convert.ToInt16(_usuarioService.GetUsuarioId());
                await _turmaRepository.CadastrarAsync(turma);
                return new Result { Success = true };
            }
            catch (Exception)
            {
                return new Result { Success = false, ErrorMessage = "Erro ao realizar cadastro." };
            }
        }
        public async Task<Result> AtualizarAsync(Turma turma)
        {
            try
            {
                var turmaExistente = await _turmaRepository.BuscarPorIdAsync(turma.IdTurma);
                if (turmaExistente == null)
                    return new Result { Success = false, ErrorMessage = "Turma não encontrada." };

                if (string.IsNullOrEmpty(turma.Nome) || turma.Nome.Length < 3)
                    return new Result { Success = false, ErrorMessage = "Nome da Turma precisa ter mais que 3 caracteres" };

                turmaExistente.Nome = turma.Nome;
                turmaExistente.Descricao = turma.Descricao;
                turmaExistente.IdUsuarioAlteracao = Convert.ToInt16(_usuarioService.GetUsuarioId());


                await _turmaRepository.AtualizarAsync(turmaExistente);
                return new Result { Success = true };
            }
            catch (Exception)
            {
                return new Result { Success = false, ErrorMessage = "Erro ao realizar cadastro." };
            }
        }
        public async Task<Result> RemoverAsync(int idTurma)
        {
            try
            {
                var turmaExistente = await _turmaRepository.BuscarPorIdAsync(idTurma);
                if (turmaExistente == null)
                    return new Result { Success = false, ErrorMessage = "Turma não encontrada." };

                var resultado = await _turmaRepository.RemoverAsync(idTurma);
                return new Result { Success = true };


            }
            catch (Exception)
            {

                return new Result { Success = false, ErrorMessage = "Erro ao realizar remoção." };
            }


        }
    }
}
