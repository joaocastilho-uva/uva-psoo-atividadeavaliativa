namespace ArteConexao.Models
{
    public class ItemReserva
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ReservaId { get; set; }
        public Guid StandId { get; set; }
        public string ImagemUrl { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
    }
}
