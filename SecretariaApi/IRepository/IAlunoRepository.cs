using SecretariaApi.Dto;
using SecretariaApi.Models;

namespace SecretariaApi.IRepository
{
    public interface IAlunoRepository
    {
        Task CadastrarAsync(AlunoUsuarioDto aluno);
        Task AtualizarAsync(AlunoUsuarioDto aluno);
        Task<bool> RemoverAsync(AlunoUsuarioDto aluno);
        Task<IEnumerable<AlunoUsuarioDto>> BuscarPorNomeAsync(string nomeAluno, int pageNumber = 1, int pageSize = 1);
        Task<AlunoUsuarioDto> BuscarPorIdAsync(int? idAluno);
    }
}
