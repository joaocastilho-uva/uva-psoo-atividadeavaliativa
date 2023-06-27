namespace ArteConexao.ViewModels
{
    public class ItemCatalogoViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid CarrinhoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
    }
}
