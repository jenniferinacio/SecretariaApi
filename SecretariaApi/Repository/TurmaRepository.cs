using System.Data;
using Dapper;
using SecretariaApi.Dto;
using SecretariaApi.IRepository;
using SecretariaApi.Models;

namespace SecretariaApi.Repository
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly IDbConnection _connection;

        public TurmaRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<TurmaDto>> BuscarTurmaQtdAlunos()
        {
            var query = @"
            SELECT t.id_turma, t.nome_turma, t.descricao_turma, 
                   COUNT(m.id_aluno) AS QtdAlunos
            FROM Turma t
            LEFT JOIN Matricula m ON m.id_turma = t.id_turma
            GROUP BY t.id_turma, t.nome_turma, t.descricao_turma
            ORDER BY t.nome_turma ASC";

            return await _connection.QueryAsync<TurmaDto>(query);
        }
        public async Task<Turma> BuscarPorIdAsync(int id)
        {
            var query = "SELECT id_turma as IdTurma, nome_turma as Nome, descricao_turma as Descricao,dt_inclusao as DtInclusao, id_usuario_inclusao as IdUsuarioInclusao,dt_alteracao as DtAlteracao, id_usuario_alteracao as IdUsuarioAlteracao FROM Turma WHERE id_turma = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Turma>(query, new { Id = id });
        }
        public async Task<IEnumerable<TurmaDto>> BuscarPorNomeAsync(string nome, int pageNumber = 1, int pageSize = 10)
        {
            var query = @"
            SELECT t.id_turma AS IdTurma, t.nome_turma as NomeTurma, t.descricao_turma as DescricaoTurma, 
                COUNT(m.id_aluno) AS QtdAlunos
                FROM Turma t
                LEFT JOIN Matricula m ON m.id_turma = t.id_turma
                WHERE nome_turma LIKE @Nome 
                GROUP BY t.id_turma, t.nome_turma, t.descricao_turma
                ORDER BY t.nome_turma ASC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            return await _connection.QueryAsync<TurmaDto>(query, new
            {
                Nome = $"%{nome}%",
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            });

        }
        public async Task<int> CadastrarAsync(Turma turma)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                var query = @"INSERT INTO Turma (nome_turma, descricao_turma, dt_inclusao, id_usuario_inclusao)
                      VALUES (@NomeTurma, @DescricaoTurma, GETDATE(), @IdUsuarioInclusao);
                      SELECT CAST(SCOPE_IDENTITY() as int)";

                var idTurma = await _connection.QuerySingleAsync<int>(query, new
                {
                    NomeTurma = turma.Nome,
                    DescricaoTurma = turma.Descricao,
                    IdUsuarioInclusao = turma.IdUsuarioInclusao
                }, transaction);
                transaction.Commit();

                return idTurma;
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
        public async Task<bool> AtualizarAsync(Turma turma)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            using var transaction = _connection.BeginTransaction();

            try
            {
                var query = @"UPDATE Turma
                      SET nome_turma = @NomeTurma, descricao_turma = @DescricaoTurma, 
                          dt_alteracao = GETDATE(), id_usuario_alteracao = @IdUsuarioAlteracao
                      WHERE id_turma = @IdTurma";

                var result = await _connection.ExecuteAsync(query, new
                {
                    NomeTurma = turma.Nome,
                    DescricaoTurma = turma.Descricao,
                    IdUsuarioAlteracao = turma.IdUsuarioAlteracao,
                    IdTurma = turma.IdTurma
                }, transaction);
                transaction.Commit();

                return result > 0;
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
        public async Task<bool> RemoverAsync(int id)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            using var transaction = _connection.BeginTransaction();
            try
            {
                var query = "DELETE FROM Turma WHERE id_turma = @Id";
                var result = await _connection.ExecuteAsync(query, new { Id = id }, transaction);
                transaction.Commit();

                return result > 0;
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
    }
}
