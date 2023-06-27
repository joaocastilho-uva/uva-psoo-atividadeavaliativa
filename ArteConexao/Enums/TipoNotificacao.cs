using System.ComponentModel;

namespace ArteConexao.Enums
{
    public enum TipoNotificacao
    {
        [Description("Sucesso")]
        Sucesso = 1,

        [Description("Informativa")]
        Informativa = 2,

        [Description("Erro")]
        Erro = 3
    }
}
