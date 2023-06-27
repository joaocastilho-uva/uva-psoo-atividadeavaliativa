using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.User
{
    public class VisualizacaoCarrinhoModel : PageModel
    {
        private readonly ICarrinhoRepository carrinhoRepository;
        private readonly IItemCarrinhoRepository itemCarrinhoRepository;
        private readonly IProdutoRepository produtoRepository;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public CarrinhoViewModel CarrinhoViewModel { get; set; }

        [BindProperty]
        public List<ItemCarrinhoViewModel> ItensCarrinhoViewModel { get; set; }

        public VisualizacaoCarrinhoModel(ICarrinhoRepository carrinhoRepository,
            IItemCarrinhoRepository itemCarrinhoRepository,
            IProdutoRepository produtoRepository,
            UserManager<IdentityUser> userManager)
        {
            this.carrinhoRepository = carrinhoRepository;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
            this.produtoRepository = produtoRepository;
            this.userManager = userManager;
            ItensCarrinhoViewModel = new List<ItemCarrinhoViewModel>();
        }

        public async Task OnGet(Guid usuarioId)
        {
            GetTempData();

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
                        var carrinhoDb = await carrinhoRepository.GetAsync(CarrinhoViewModel.UsuarioId);

                        if (carrinhoDb.ItensCarrinho.Any())
                        {
                            carrinhoDb.ValorTotal = carrinhoDb.ItensCarrinho.ToList().Sum(s => (s.ValorReserva * s.Quantidade));
                            await carrinhoRepository.UpdateAsync(carrinhoDb);

                            SetTempData(TipoNotificacao.Sucesso, "Produto removido com sucesso.");
                        }
                        else
                        {
                            carrinhoDb.ValorTotal = 0;
                            await carrinhoRepository.UpdateAsync(carrinhoDb);

                            return Redirect($"/User/VisualizacaoCarrinho/{@userManager.GetUserId(User)}");
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

        private void GetTempData()
        {
            var notificaticao = (string)TempData["Notificacao"];

            if (!string.IsNullOrWhiteSpace(notificaticao))
            {
                ViewData["Notificacao"] = JsonSerializer.Deserialize<NotificacaoViewModel>(notificaticao.ToString()); ;
            }
        }

        private void SetTempData(TipoNotificacao tipoNotificacao, string mensagem)
        {
            var notificacao = new NotificacaoViewModel
            {
                Tipo = tipoNotificacao,
                Mensagem = mensagem
            };

            TempData["Notificacao"] = JsonSerializer.Serialize(notificacao);
        }
    }
}
