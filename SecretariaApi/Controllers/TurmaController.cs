using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaApi.Dto;
using SecretariaApi.IService;
using SecretariaApi.Models;
using SecretariaApi.Service;
using SecretariaApi.Util;

namespace SecretariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TurmaController : Controller
    {
        private readonly ITurmaService _turmaService;

        public TurmaController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }
        [HttpGet("BuscarPorNomeAsync")]
        public async Task<ActionResult> BuscarPorNomeAsync(string nome, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var alunos = await _turmaService.BuscarPorNomeAsync(nome, pageNumber, pageSize);
                return Ok(alunos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
        [HttpPost("CadastrarTurmaAsync")]
        public async Task<ActionResult> CadastrarTurmaAsync([FromBody] Turma turma)
        {
            try
            {
                var resultado = await _turmaService.CadastrarAsync(turma);

                if (!resultado.Success)
                    return BadRequest(new { message = resultado.ErrorMessage });

                return Ok(new { message = "Cadastro realizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
        [HttpPut("AtualizarTurmaAsync")]
        public async Task<IActionResult> AtualizarAlunoAsync([FromBody] Turma turma)
        {
            var resultado = await _turmaService.AtualizarAsync(turma);

            if (!resultado.Success)
                return BadRequest(new { message = resultado.ErrorMessage });

            return Ok(new { message = "Turma atualizado com sucesso!" });
        }
        [HttpDelete("RemoverTurmaAsync/{idTurma}")]
        public async Task<ActionResult<Result>> RemoverTurmaAsync(int idTurma)
        {
            try
            {
                var resultado = await _turmaService.RemoverAsync(idTurma);

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
