using SecretariaApi.Util;

namespace SecretariaApi.IService
{
    public interface IUsuarioService
    {
        Task<ResultLogin> Login(string email, string senha);
        string GetUsuarioId();
    }
}
