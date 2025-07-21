using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaApi.Dto;
using SecretariaApi.IService;
using SecretariaApi.Models;
using SecretariaApi.Util;

namespace SecretariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AlunoController : Controller
    {
        private readonly IAlunoService _alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet("BuscarPorNomeAsync")]
        public async Task<ActionResult> BuscarPorNomeAsync(string nome,int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var alunos = await _alunoService.BuscarPorNomeAsync(nome,pageNumber, pageSize);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
        [HttpPost("CadastrarAlunoAsync")]
        public async Task<ActionResult> CadastrarAlunoAsync([FromBody] AlunoUsuarioDto alunoDto)
        {
            try
            {
                var resultado = await _alunoService.CadastrarAsync(alunoDto);

                if (!resultado.Success)
                    return BadRequest(new { message = resultado.ErrorMessage });

                return Ok(new { message = "Cadastro realizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
        [HttpPut("AtualizarAlunoAsync")]
        public async Task<IActionResult> AtualizarAlunoAsync([FromBody] AlunoUsuarioDto alunoDto)
        {
            var resultado = await _alunoService.AtualizarAsync(alunoDto);

            if (!resultado.Success)
                return BadRequest(new { message = resultado.ErrorMessage });

            return Ok(new { message = "Aluno atualizado com sucesso!" });
        }
        [HttpDelete("RemoverAlunoAsync")]
        public async Task<ActionResult<Result>> RemoverAlunoAsync([FromBody] AlunoUsuarioDto alunoDto)
        {
            try
            {
                var resultado = await _alunoService.RemoverAsync(alunoDto);

                if (!resultado.Success)
                {
                    return BadRequest(resultado.ErrorMessage);
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro ao tentar remover o aluno.", Details = ex.Message });
            }
        }
    }
}
