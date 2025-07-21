using SecretariaApi.Dto;
using System.Data;
using Dapper;
using SecretariaApi.IRepository;

namespace SecretariaApi.Repository
{
    public class MatriculaRepository: IMatriculaRepository
    {
        private readonly IDbConnection _connection;
        public MatriculaRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task CadastrarAsync(MatriculaDto matriculaDto)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
			{
                var query = "INSERT INTO Matricula (id_aluno, id_turma) VALUES (@IdAluno, @IdTurma)";
                await _connection.ExecuteAsync(query, new { IdAluno = matriculaDto.IdAluno, IdTurma = matriculaDto.IdTurma }, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
        public async Task<int> BuscarMatriculaExistente(MatriculaDto matriculaDto)
        {
             return await _connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Matricula WHERE id_aluno = @IdAluno AND id_turma = @IdTurma",
                new { IdAluno = matriculaDto.IdAluno, IdTurma = matriculaDto.IdTurma });
        }
        public async Task<int> GetTotalAlunosMatriculados(int idTurma)
        {
           
            var query = "SELECT COUNT(*) FROM Matricula WHERE id_turma = @IdTurma";
            return await _connection.ExecuteScalarAsync<int>(query, new { IdTurma = idTurma });
            
        }
        public async Task<List<AlunoMatriculaDto>> BuscarAlunosMatriTurma(int idTurma, int pageNumber = 1, int pageSize = 10)
        {
            var query = @"
                SELECT a.id_aluno as IdAluno, a.nome as Nome, a.cpf as Cpf
                FROM Aluno a
                INNER JOIN Matricula m ON a.id_aluno = m.id_aluno
                WHERE m.id_turma = @IdTurma
                ORDER BY a.nome ASC
               OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            var alunos = await _connection.QueryAsync<AlunoMatriculaDto>(query, new
            {
                IdTurma = idTurma,
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            });
            return alunos.ToList();
        }
    }
}
