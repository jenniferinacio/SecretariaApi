using System.ComponentModel;

namespace SecretariaApi.Util.Enum
{
    public enum TipoUsuario
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Aluno")]
        Aluno = 2
    }
}
