using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretariaApi.Dto;
using SecretariaApi.IService;
using SecretariaApi.Service;

namespace SecretariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MatriculaController : Controller
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculaController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }
        [HttpPost("CadastrarMatriculaAsync")]
        public async Task<ActionResult> CadastrarAlunoAsync([FromBody] MatriculaDto matriculaDto)
        {
            try
            {
                var resultado = await _matriculaService.CadastrarAsync(matriculaDto);

                if (!resultado.Success)
                    return BadRequest(new { message = resultado.ErrorMessage });

                return Ok(new { message = "Cadastro realizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
        [HttpGet("BuscarAlunosMatriTurmaAsync")]
        public async Task<ActionResult> BuscarAlunosMatriTurmaAsync(int idTurma, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var alunos = await _matriculaService.BuscarAlunosMatriTurma(idTurma, pageNumber, pageSize);

                int total = await _matriculaService.GetTotalAlunosMatriculados(idTurma);
                return Ok(new
                {
                    TotalAlunos = total,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize),
                    CurrentPage = pageNumber,
                    Alunos = alunos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a requisição.", detail = ex.Message });
            }
        }
    }
}
