using System.Data;
using SecretariaApi.Models;
using Dapper;
using SecretariaApi.IRepository;

namespace SecretariaApi.Repository
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly IDbConnection _dbConnection;

        public UsuarioRepository( IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<Usuario> BuscarUsuarioEmail(string email)
        {
            var queryUsuario = "SELECT id_usuario as IdUsuario ,email as Email,senha as Senha,tipo_usuario as TipoUsuario FROM Usuario WHERE email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, new { Email = email });
        }

    }
}
