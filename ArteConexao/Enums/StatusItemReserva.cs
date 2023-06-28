using System.ComponentModel;

namespace ArteConexao.Enums
{
    public enum StatusItemReserva
    {
        [Description("Processada")]
        Processada = 1,

        [Description("Confirmada")]
        Confirmada = 2,

        [Description("Finalizada")]
        Finalizada = 3,
    }
}
