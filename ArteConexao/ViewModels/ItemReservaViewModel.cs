using ArteConexao.Enums;

namespace ArteConexao.ViewModels
{
    public class ItemReservaViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ReservaId { get; set; }
        public Guid StandId { get; set; }
        public StatusItemReserva Status { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
    }
}
