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
        public List<ItemCarrinhoViewModel> ItensCarrinhoViewModel { get; set; }

        public VisualizacaoCarrinhoModel(ICarrinhoRepository carrinhoRepository,
            IItemCarrinhoRepository itemCarrinhoRepository,
            IProdutoRepository produtoRepository)
        {
            this.carrinhoRepository = carrinhoRepository;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
            this.produtoRepository = produtoRepository;

            ItensCarrinhoViewModel = new List<ItemCarrinhoViewModel>();
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
                };

                if (carrinho.ItensCarrinho.Any())
                {
                    foreach (var item in carrinho.ItensCarrinho)
                    {
                        var nome = (await produtoRepository.GetAsync(item.ProdutoId)).Nome;

                        ItensCarrinhoViewModel.Add(new ItemCarrinhoViewModel()
                        {
                            Id = item.Id,
                            ProdutoId = item.ProdutoId,
                            ImagemUrl = item.ImagemUrl,
                            Nome = nome,
                            Quantidade = item.Quantidade,
                            StandId = item.StandId,
                            ValorReserva = item.ValorReserva
                        });
                    }

                    CarrinhoViewModel.ItensCarrinhoViewModel = ItensCarrinhoViewModel;
                }
                else
                {
                    SetViewData(TipoNotificacao.Informativa, "O carrinho se encontra vazio.");
                }
            }
            else
            {
                SetViewData(TipoNotificacao.Informativa, "O carrinho se encontra vazio.");
            }
        }

        public async Task<IActionResult> OnPostDelete(Guid itemCarrinhoId)
        {
            try
            {
                ValidateOnPostDelete();

                if (itemCarrinhoId != Guid.Empty)
                {
                    var excluido = await itemCarrinhoRepository.DeleteAsync(itemCarrinhoId);

                    if (excluido)
                    {
                        var carrinhoDb = await carrinhoRepository.GetAsync(CarrinhoViewModel.Id);

                        if (carrinhoDb.ItensCarrinho.Any())
                        {
                            carrinhoDb.ValorTotal += carrinhoDb.ItensCarrinho.ToList().Sum(s => (s.ValorReserva * s.Quantidade));
                            await carrinhoRepository.UpdateAsync(carrinhoDb);

                            SetViewData(TipoNotificacao.Sucesso, "Produto removido com sucesso.");
                        }
                        else
                        {
                            carrinhoDb.ValorTotal = 0;
                            await carrinhoRepository.UpdateAsync(carrinhoDb);

                            //return RedirectToPage($"/User/VisualizacaoCarrinho/@userManager.GetUserId(User)");
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível remover o produto: {ex.Message}.");

                return Page();
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
