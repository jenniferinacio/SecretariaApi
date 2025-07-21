using SecretariaApi.Dto;
using SecretariaApi.Util;

namespace SecretariaApi.IService
{
    public interface IMatriculaService
    {
        Task<Result> CadastrarAsync(MatriculaDto matriculaDto);
        Task<int> GetTotalAlunosMatriculados(int idTurma);
        Task<List<AlunoMatriculaDto>> BuscarAlunosMatriTurma(int idTurma, int pageNumber = 1, int pageSize = 10);
    }
}
