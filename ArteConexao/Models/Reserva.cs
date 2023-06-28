namespace ArteConexao.Models
{
    public class Reserva
    {
        public Reserva()
        {
            this.ItensReserva = new List<ItemReserva>();
        }

        public Guid Id { get; set; }
        public decimal ValorTotal { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime DataInclusao { get; set; }
        public ICollection<ItemReserva> ItensReserva { get; set; }
    }
}
