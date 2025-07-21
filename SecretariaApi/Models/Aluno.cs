namespace SecretariaApi.Models
{
    public class Aluno
    {
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public DateTime DtNacimento { get; set; }
        public string Cpf { get; set; }
        public DateTime DtInclusao { get; set; }
        public string IdUsuarioInclusao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public string IdUsuarioAlteracao { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

    }
}
