using SecretariaApi.Models;
using SecretariaApi.IRepository;
using SecretariaApi.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SecretariaApi.IService;
using SecretariaApi.Util.Enum;

namespace SecretariaApi.Service
{
    public class UsuarioService: IUsuarioService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(IConfiguration configuration, IUsuarioRepository usuarioRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultLogin> Login(string email, string senha)
        {
            var usuario = await _usuarioRepository.BuscarUsuarioEmail(email);
            
            if(usuario ==null)
                return new ResultLogin { Success = false, ErrorMessage = "Login Invalido." };

            if (!SenhaHelper.VerificarSenhaLogin(senha, usuario.Senha))
                return new ResultLogin { Success = false, ErrorMessage = "Login Invalido." };

            var token = GenerateToken(usuario);

            return new ResultLogin { Success = true,Token=token };

        }
        public string GetUsuarioId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;  
        }

        public string GenerateToken(Usuario usuario)
        {
            var role = Helper.GetEnumDescription((TipoUsuario)usuario.TipoUsuario);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),  
                new Claim(ClaimTypes.Name, usuario.Email),                       
                new Claim(ClaimTypes.Role, role.ToString())         
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

