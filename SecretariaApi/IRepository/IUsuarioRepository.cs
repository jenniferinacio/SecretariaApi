using SecretariaApi.Models;

namespace SecretariaApi.IRepository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> BuscarUsuarioEmail(string email);
    }
}
