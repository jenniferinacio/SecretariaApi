using SecretariaApi.Dto;

namespace SecretariaApi.IRepository
{
    public interface IMatriculaRepository
    {
        Task<List<AlunoMatriculaDto>> BuscarAlunosMatriTurma(int idTurma, int pageNumber = 1, int pageSize = 10);
        Task CadastrarAsync(MatriculaDto matriculaDto);
        Task<int> BuscarMatriculaExistente(MatriculaDto matriculaDto);
        Task<int> GetTotalAlunosMatriculados(int idTurma);
    }
}
