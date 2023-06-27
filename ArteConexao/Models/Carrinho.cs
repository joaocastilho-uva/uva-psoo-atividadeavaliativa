namespace ArteConexao.Models
{
    public class Carrinho
    {
        public Carrinho()
        {
            this.ItensCarrinho = new List<ItemCarrinho>();
        }

        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public ICollection<ItemCarrinho> ItensCarrinho { get; set; }
    }
}
