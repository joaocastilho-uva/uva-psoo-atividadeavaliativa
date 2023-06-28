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
        private readonly IReservaRepository reservaRepository;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public CarrinhoViewModel CarrinhoViewModel { get; set; }

        [BindProperty]
        public List<ItemCarrinhoViewModel> ItensCarrinhoViewModel { get; set; }

        [BindProperty]
        public Guid CarrinhoId { get; set; }

        public VisualizacaoCarrinhoModel(ICarrinhoRepository carrinhoRepository,
            IItemCarrinhoRepository itemCarrinhoRepository,
            IProdutoRepository produtoRepository,
            IReservaRepository reservaRepository,
            UserManager<IdentityUser> userManager)
        {
            this.carrinhoRepository = carrinhoRepository;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
            this.produtoRepository = produtoRepository;
            this.reservaRepository = reservaRepository;
            this.userManager = userManager;

            ItensCarrinhoViewModel = new List<ItemCarrinhoViewModel>();
        }

        public async Task OnGet(Guid usuarioId)
        {
            var carrinho = await carrinhoRepository.GetAsync(usuarioId);

            if (carrinho != null)
            {
                CarrinhoId = carrinho.Id;

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
                            StandId = item.StandId,
                            Nome = nome,
                            ImagemUrl = item.ImagemUrl,
                            Quantidade = item.Quantidade,
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

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ItensCarrinhoViewModel.Any())
                {
                    var reserva = new Reserva()
                    {
                        DataInclusao = DateTime.Now,
                        UsuarioId = new Guid(userManager.GetUserId(User)),
                        ValorTotal = ItensCarrinhoViewModel.Sum(s => s.Quantidade * s.ValorReserva)
                    };

                    await reservaRepository.AddAsync(reserva);

                    if (reserva != null && reserva.Id != Guid.Empty)
                    {
                        foreach (var itemCarrinhoViewModel in ItensCarrinhoViewModel)
                        {
                            var itemReserva = new ItemReserva()
                            {
                                ReservaId = reserva.Id,
                                StandId = itemCarrinhoViewModel.StandId,
                                ProdutoId = itemCarrinhoViewModel.ProdutoId,
                                Quantidade = itemCarrinhoViewModel.Quantidade,
                                Status = StatusItemReserva.Processada,
                                ValorReserva = itemCarrinhoViewModel.ValorReserva
                            };

                            reserva.ItensReserva.Add(itemReserva);
                        }

                        await reservaRepository.UpdateAsync(reserva);
                        await carrinhoRepository.DeleteAsync(CarrinhoId);
                        return RedirectToPage("/User/FinalizacaoReserva");
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível finalizar a reserva: {ex.Message}");
                return Page();
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

                            SetViewData(TipoNotificacao.Informativa, "Produto removido com sucesso.");
                        }
                        else
                        {
                            await carrinhoRepository.DeleteAsync(carrinhoDb.Id);
                            SetViewData(TipoNotificacao.Informativa, "O carrinho se encontra vazio.");
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
