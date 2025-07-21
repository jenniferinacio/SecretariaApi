using SecretariaApi.Dto;
using SecretariaApi.Models;
using SecretariaApi.Util;

namespace SecretariaApi.IService
{
    public interface ITurmaService
    {
        Task<Result> RemoverAsync(int id);
        Task<Result> AtualizarAsync(Turma turma);
        Task<Result> CadastrarAsync(Turma turma);
        Task<IEnumerable<TurmaDto>> BuscarPorNomeAsync(string nome, int pageNumber = 1, int pageSize = 10);
    }
}
