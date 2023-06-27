using System.ComponentModel;

namespace ArteConexao.Enums
{
    public enum StatusReserva
    {
        [Description("Em processamento")]
        EmProcessamento = 1,

        [Description("Confirmada")]
        Confirmada = 2,

        [Description("Finalizada")]
        Finalizada = 3,
    }
}
