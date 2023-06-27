namespace ArteConexao.Models
{
    public class ItemReserva
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid ReservaId { get; set; }
        public Guid StandId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
