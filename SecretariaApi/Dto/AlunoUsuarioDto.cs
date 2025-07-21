namespace SecretariaApi.Dto
{
    public class AlunoUsuarioDto
    {
        public int? IdAluno { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int? IdUsuarioInclusao { get; set; } 
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DtNascimento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int TipoUsuario { get; set; }
    }
}
