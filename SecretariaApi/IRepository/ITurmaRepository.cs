using SecretariaApi.Dto;
using SecretariaApi.Models;

namespace SecretariaApi.IRepository
{
    public interface ITurmaRepository
    {
        Task<IEnumerable<TurmaDto>> BuscarTurmaQtdAlunos();
        Task<Turma> BuscarPorIdAsync(int id);
        Task<bool> RemoverAsync(int id);
        Task<bool> AtualizarAsync(Turma turma);
        Task<int> CadastrarAsync(Turma turma);
        Task<IEnumerable<TurmaDto>> BuscarPorNomeAsync(string nome, int pageNumber = 1, int pageSize = 10);
    }
}
