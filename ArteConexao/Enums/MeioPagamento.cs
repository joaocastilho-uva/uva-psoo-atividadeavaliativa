using System.ComponentModel;

namespace ArteConexao.Enums
{
    public enum MeioPagamento
    {
        [Description("Cartão de crédito")]
        Cartao = 1,

        [Description("Pix")]
        Pix = 2
    }
}
