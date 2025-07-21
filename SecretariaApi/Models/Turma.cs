namespace SecretariaApi.Models
{
    public class Turma
    {
        public int IdTurma { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DtInclusao { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public int IdUsuarioAlteracao { get; set; }

    }
}
