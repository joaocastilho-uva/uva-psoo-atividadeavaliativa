using ArteConexao.Models;

namespace ArteConexao.ViewModels
{
    public class CarrinhoViewModel
    {
        public CarrinhoViewModel()
        {
            this.ItensCarrinhoViewModel = new List<ItemCarrinhoViewModel>();
        }

        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public ICollection<ItemCarrinhoViewModel> ItensCarrinhoViewModel { get; set; }
    }
}
