namespace SecretariaApi.Dto
{
    public class TurmaDto
    {
        public int IdTurma { get; set; }
        public string NomeTurma { get; set; }
        public string DescricaoTurma { get; set; }
        public DateTime? DtInclusao { get; set; }
        public int? IdUsuarioInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public int QtdAlunos { get; set; }
    }
}
