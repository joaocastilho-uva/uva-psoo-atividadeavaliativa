using ArteConexao.Enums;

namespace ArteConexao.ViewModels
{
    public class ReservaViewModel
    {
        public Guid Id { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusReserva Status { get; set; }
        public Guid Usuario { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}
