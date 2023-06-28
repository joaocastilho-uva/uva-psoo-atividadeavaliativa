using ArteConexao.Enums;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class GerenciamentoReservaModel : PageModel
    {
        private readonly IItemReservaRepository itemReservaRepository;
        private readonly IProdutoRepository produtoRepository;
        private readonly UserManager<IdentityUser> userManager;

        public List<ItemReservaViewModel> ItensReservaViewModel { get; set; }

        [BindProperty]
        public Guid StandId { get; set; }

        public GerenciamentoReservaModel(IItemReservaRepository itemReservaRepository,
            IProdutoRepository produtoRepository,
            UserManager<IdentityUser> userManager)
        {
            this.itemReservaRepository = itemReservaRepository;
            this.produtoRepository = produtoRepository;
            this.userManager = userManager;

            ItensReservaViewModel = new List<ItemReservaViewModel>();
        }

        public async Task OnGet(Guid standId)
        {
            GetTempData();

            StandId = standId;

            var itensReserva = await itemReservaRepository.GetAllAsync(standId);

            if (itensReserva.Any())
            {
                foreach (var itemReserva in itensReserva)
                {
                    var produtoDb = await produtoRepository.GetAsync(itemReserva.ProdutoId);

                    ItensReservaViewModel.Add(new ItemReservaViewModel()
                    {
                        Id = itemReserva.Id,
                        Nome = produtoDb.Nome,
                        ProdutoId = itemReserva.ProdutoId,
                        Status = itemReserva.Status,
                        Quantidade = itemReserva.Quantidade,
                        ValorReserva = itemReserva.ValorReserva,
                        ValorTotal = itemReserva.ValorTotal
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostProcessamento(Guid itemReservaId)
        {
            try
            {
                var itemReservaDb = await itemReservaRepository.GetAsync(itemReservaId);

                if (itemReservaDb != null)
                {
                    if (itemReservaDb.Status == StatusItemReserva.Processada)
                    {
                        itemReservaDb.Status = StatusItemReserva.Confirmada;
                        SetTempData(TipoNotificacao.Sucesso, "Item de reserva confirmado com sucesso.");
                    }
                    else if(itemReservaDb.Status == StatusItemReserva.Confirmada)
                    {
                        itemReservaDb.Status = StatusItemReserva.Finalizada;
                        SetTempData(TipoNotificacao.Sucesso, "Item de reserva finalizado com sucesso.");
                    }
                    else
                    {
                        SetTempData(TipoNotificacao.Informativa, "Item de reserva já finalizado.");
                    }

                    await itemReservaRepository.UpdateAsync(itemReservaDb);
                    return Redirect($"/Admin/GerenciamentoReserva/{StandId}");
                }

                return Page();
            }
            catch (Exception)
            {
                return Page();
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

        private void GetTempData()
        {
            var notificaticao = (string)TempData["Notificacao"];

            if (!string.IsNullOrWhiteSpace(notificaticao))
            {
                ViewData["Notificacao"] = JsonSerializer.Deserialize<NotificacaoViewModel>(notificaticao.ToString()); ;
            }
        }
    }
}
