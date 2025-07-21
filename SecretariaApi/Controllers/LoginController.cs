using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaApi.IService;
using SecretariaApi.Service;
using SecretariaApi.Util;

namespace SecretariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var resultado = await _usuarioService.Login(email, senha);

            if (!resultado.Success)
                return BadRequest(new { message = resultado.ErrorMessage });

            return Ok(new { message = resultado });

        }       
    }
}
