using SecretariaApi.Dto;
using SecretariaApi.Models;
using SecretariaApi.Util;

namespace SecretariaApi.IService
{
    public interface IAlunoService
    {
        Task<Result> CadastrarAsync(AlunoUsuarioDto aluno);
        Task<Result> AtualizarAsync(AlunoUsuarioDto alunoDto);
        Task<Result> RemoverAsync(AlunoUsuarioDto aluno);
        Task<IEnumerable<AlunoUsuarioDto>> BuscarPorNomeAsync(string nomeAluno, int pageNumber = 1, int pageSize = 1);
        Task<AlunoUsuarioDto?> BuscarPorIdAsync(int id);
    }
}
