using ArteConexao.Enums;

namespace ArteConexao.ViewModels
{
    public class ProdutoViewModel
    {
        public Guid Id { get; set; }
        public Guid StandId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl { get; set; }
        public Pais PaisOrigem { get; set; }
        public Categoria Categoria { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public int QuantidadeTotal { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorReserva { get; set; }
        public Guid? UsuarioInclusao { get; set; }
        public DateTime? DataInclusao { get; set; }
        public Guid? UsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
