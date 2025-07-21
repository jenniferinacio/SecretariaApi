using SecretariaApi.Dto;
using SecretariaApi.IRepository;
using SecretariaApi.IService;
using SecretariaApi.Util;

namespace SecretariaApi.Service
{
    public class MatriculaService: IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        public MatriculaService(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public async Task<List<AlunoMatriculaDto>> BuscarAlunosMatriTurma(int idTurma, int pageNumber = 1, int pageSize = 10)
        {
            return await _matriculaRepository.BuscarAlunosMatriTurma(idTurma, pageNumber, pageSize);
        }
        public async Task<int> GetTotalAlunosMatriculados(int idTurma)
        {
            return await _matriculaRepository.GetTotalAlunosMatriculados(idTurma);
        }
        public async Task<Result> CadastrarAsync(MatriculaDto matriculaDto)
        {
            try
            {
                var existeMatricula = await _matriculaRepository.BuscarMatriculaExistente(matriculaDto);

                if (existeMatricula > 0)
                    return new Result { Success = false, ErrorMessage = "Aluno já Matriculado nessa Turma." };

                await _matriculaRepository.CadastrarAsync(matriculaDto);
                return new Result { Success = true };

            }
            catch (Exception)
            {
                return new Result { Success = false, ErrorMessage = "Erro ao realizar cadastro." };
            }

        }
    }
}
