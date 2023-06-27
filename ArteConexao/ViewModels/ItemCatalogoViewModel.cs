using ArteConexao.Enums;

namespace ArteConexao.ViewModels
{
    public class ItemCatalogoViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid CarrinhoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Pais PaisOrigem { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
    }
}
