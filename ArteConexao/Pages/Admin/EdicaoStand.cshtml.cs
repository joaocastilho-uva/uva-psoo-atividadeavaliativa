using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class EdicaoStandModel : PageModel
    {
        private readonly IStandRepository standRepository;

        [BindProperty]
        public StandViewModel StandViewModel { get; set; }

        public EdicaoStandModel(IStandRepository standRepository)
        {
            this.standRepository = standRepository;
        }

        public async Task OnGet(Guid standId)
        {
            var standDb = await standRepository.GetAsync(standId);

            if (standDb != null)
            {
                StandViewModel = new StandViewModel()
                {
                    Id = standDb.Id,
                    Nome = standDb.Nome,
                    Localizacao = standDb.Localizacao
                };
            }
        }

        public async Task<IActionResult> OnPost(Guid id)
        {
            try
            {
                ValidateOnPost();

                if (ModelState.IsValid)
                {
                    var stand = new Stand()
                    {
                        Id = StandViewModel.Id,
                        Nome = StandViewModel.Nome,
                        Localizacao = StandViewModel.Localizacao
                    };

                    await standRepository.UpdateAsync(stand);

                    SetTempData(TipoNotificacao.Sucesso, "Stand atualizado com sucesso.");

                    return RedirectToPage("/admin/gerenciamentostand");
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível atualizar o stand: {ex.Message}.");

                return Page();
            }
        }

        private void ValidateOnPost()
        {
            if (string.IsNullOrWhiteSpace(StandViewModel.Nome))
            {
                throw new Exception("O campo nome é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(StandViewModel.Localizacao))
            {
                throw new Exception("O campo localização é obrigatório.");
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
