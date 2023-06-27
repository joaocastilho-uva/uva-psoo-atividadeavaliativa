using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArteConexao.Pages.User
{
    public class VisualizacaoCarrinhoModel : PageModel
    {
        private readonly ICarrinhoRepository carrinhoRepository;
        private readonly IItemCarrinhoRepository itemCarrinhoRepository;
        private readonly IProdutoRepository produtoRepository;

        [BindProperty]
        public CarrinhoViewModel CarrinhoViewModel { get; set; }

        [BindProperty]
        public ItemCarrinhoViewModel ItemCarrinhoViewModel { get; set; }

        public List<Produto> Produtos { get; set; }

        public VisualizacaoCarrinhoModel(ICarrinhoRepository carrinhoRepository,
            IItemCarrinhoRepository itemCarrinhoRepository,
            IProdutoRepository produtoRepository)
        {
            this.carrinhoRepository = carrinhoRepository;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
            this.produtoRepository = produtoRepository;

            Produtos = new List<Produto>();
        }

        public async Task OnGet(Guid usuarioId)
        {
            var carrinho = await carrinhoRepository.GetAsync(usuarioId);

            if (carrinho != null)
            {
                CarrinhoViewModel = new CarrinhoViewModel()
                {
                    Id = carrinho.Id,
                    UsuarioId = carrinho.UsuarioId,
                    ValorTotal = carrinho.ValorTotal,
                    //ItensCarrinho = carrinho.ItensCarrinho
                };

                if (carrinho.ItensCarrinho.Any())
                {
                    foreach (var item in carrinho.ItensCarrinho)
                    {
                        Produtos.Add(await produtoRepository.GetAsync(item.ProdutoId));
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostDelete(Guid produtoId)
        {
            try
            {
                ValidateOnPostDelete();

                if (CarrinhoViewModel.ItensCarrinhoViewModel.Any())
                {
                    var itemCarrinhoId = CarrinhoViewModel.ItensCarrinhoViewModel.FirstOrDefault(w => w.ProdutoId == produtoId).ProdutoId;

                    if (itemCarrinhoId != Guid.Empty)
                    {
                        var excluido = await itemCarrinhoRepository.DeleteAsync(itemCarrinhoId);

                        if (excluido)
                        {
                            var carrinhoDb = await carrinhoRepository.GetAsync(CarrinhoViewModel.Id);
                            carrinhoDb.ValorTotal += carrinhoDb.ItensCarrinho.ToList().Sum(s => (s.Valor * s.Quantidade));

                            await carrinhoRepository.UpdateAsync(carrinhoDb);
                            SetViewData(TipoNotificacao.Sucesso, "Produto removido com sucesso.");
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível remover o produto: {ex.Message}.");

                return RedirectToPage("/admin/gerenciamentoproduto");
            }
        }

        private void ValidateOnPostDelete()
        {

        }

        private void SetViewData(TipoNotificacao tipoNotificacao, string mensagem)
        {
            var notificacao = new NotificacaoViewModel
            {
                Tipo = tipoNotificacao,
                Mensagem = mensagem
            };

            ViewData["Notificacao"] = notificacao;
        }
    }
}
