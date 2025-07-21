using System.Data;
using Dapper;
using SecretariaApi.Dto;
using SecretariaApi.IRepository;
using SecretariaApi.Models;

namespace SecretariaApi.Repository
{
    public class AlunoRepository: IAlunoRepository
    {
        private readonly IDbConnection _connection;
        public AlunoRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<AlunoUsuarioDto>> BuscarPorNomeAsync(string nomeAluno,int pageNumber = 1,int pageSize = 10)
        {
            try
            {
                var sql = @"SELECT nome as Nome,dt_nacimento as DtNascimento,cpf as Cpf FROM Aluno a
                    inner join Usuario u on u.id_usuario = a.id_usuario
                    WHERE a.nome LIKE @Nome
                    ORDER BY a.nome ASC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; ";

                var alunos = await _connection.QueryAsync<AlunoUsuarioDto>(sql, new
                {
                    Nome = $"%{nomeAluno}%",
                    Offset = (pageNumber - 1) * pageSize,
                    PageSize = pageSize
                });

                return alunos;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<AlunoUsuarioDto> BuscarPorIdAsync(int? idAluno)
        {
            try
            {
                string query = @"
                SELECT 
                    a.id_aluno as IdAluno, a.nome as Nome, a.dt_nacimento as DtNascimento, a.cpf as Cpf, a.id_usuario as IdUsuario, 
                    u.email as Email, u.senha as Senha
                FROM Aluno a
                INNER JOIN Usuario u ON a.id_usuario = u.id_usuario
                WHERE a.id_aluno = @IdAluno";

                var aluno = await _connection.QuerySingleOrDefaultAsync<AlunoUsuarioDto>(query, new { IdAluno = idAluno });

                return aluno;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar aluno por ID", ex);
            }
        }
        public async Task<List<Aluno>> ListarTodosAluno(int pageNumber, int pageSize = 10)
        {
            var sql = @"SELECT nome as Nome,cpf as Cpf,dt_nacimento  as DtNascimento FROM Aluno ORDER BY nome ASC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var alunos = await _connection.QueryAsync<Aluno>(sql, new
            {
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            });
            return alunos.ToList();
        }

        public async Task CadastrarAsync(AlunoUsuarioDto alunoUsuarioDto)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                var sqlUsuario = @"
                INSERT INTO Usuario (email, senha, tipo_usuario)
                VALUES (@Email, @Senha, @TipoUsuario);
                SELECT CAST(SCOPE_IDENTITY() as int);";

                var idUsuario = await _connection.QuerySingleAsync<int>(sqlUsuario, new
                {
                    Email = alunoUsuarioDto.Email,
                    Senha = alunoUsuarioDto.Senha,
                    TipoUsuario = alunoUsuarioDto.TipoUsuario
                }, transaction);

                var sqlAluno = @"
                INSERT INTO Aluno (nome, cpf, dt_nacimento, id_usuario, dt_inclusao,id_usuario_inclusao)
                VALUES (@Nome, @Cpf, @DtNascimento, @IdUsuario, getDate(),@UsuarioInclusao);";

                await _connection.ExecuteAsync(sqlAluno, new
                {
                    Nome = alunoUsuarioDto.Nome,
                    Cpf = alunoUsuarioDto.Cpf,
                    DtNascimento = alunoUsuarioDto.DtNascimento,
                    IdUsuario = idUsuario,
                    UsuarioInclusao = alunoUsuarioDto.IdUsuarioInclusao
                }, transaction);

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

        public async Task AtualizarAsync(AlunoUsuarioDto alunoDto)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                string alunoQuery = @"
                UPDATE Aluno 
                SET 
                    nome = @Nome, 
                    dt_nacimento = @DtNascimento, 
                    cpf = @Cpf,
                    dt_alteracao = GETDATE(), 
                    id_usuario_alteracao = @IdUsuarioAlteracao
                    WHERE id_aluno = @IdAluno";

                await _connection.ExecuteAsync(alunoQuery, new
                {
                    Nome = alunoDto.Nome,
                    DtNascimento = alunoDto.DtNascimento,
                    Cpf = alunoDto.Cpf,
                    IdUsuarioAlteracao = alunoDto.IdUsuarioAlteracao,
                    IdAluno = alunoDto.IdAluno
                }, transaction);

                string usuarioQuery = @"
                UPDATE Usuario
                SET 
                    email = @Email, 
                    senha = @Senha
                    WHERE id_usuario = @IdUsuario";

                await _connection.ExecuteAsync(usuarioQuery, new
                {
                    Email = alunoDto.Email,
                    Senha = alunoDto.Senha,
                    IdUsuario = alunoDto.IdUsuario
                },transaction);

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
        public async Task<bool> RemoverAsync(AlunoUsuarioDto alunoDto)
        {
            var result = true;
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();
            try
            {
                string matriculaQuery = "DELETE FROM Matricula WHERE id_aluno = @IdAluno";
                await _connection.ExecuteAsync(matriculaQuery, new { IdAluno = alunoDto.IdAluno }, transaction);

                string alunoQuery = "DELETE FROM Aluno WHERE id_aluno = @IdAluno";
                await _connection.ExecuteAsync(alunoQuery, new { IdAluno = alunoDto.IdAluno }, transaction);

                string usuarioQuery = "DELETE FROM Usuario WHERE id_usuario = @IdUsuario";
                await _connection.ExecuteAsync(usuarioQuery, new { IdUsuario = alunoDto.IdUsuario }, transaction);

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
            return result;
        }
    }
}
