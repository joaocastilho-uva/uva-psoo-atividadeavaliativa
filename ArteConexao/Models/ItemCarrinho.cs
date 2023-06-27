namespace ArteConexao.Models
{
    public class ItemCarrinho
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid CarrinhoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
