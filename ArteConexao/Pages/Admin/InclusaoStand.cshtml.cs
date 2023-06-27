using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class InclusaoStandModel : PageModel
    {
        private readonly IStandRepository standRepository;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public StandViewModel StandViewModel { get; set; }

        public InclusaoStandModel(IStandRepository standRepository,
            UserManager<IdentityUser> userManager)
        {
            this.standRepository = standRepository;
            this.userManager = userManager;
        }

        public async Task OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                ValidateOnPost();

                if (ModelState.IsValid)
                {
                    var stand = new Stand()
                    {
                        Nome = StandViewModel.Nome,
                        Localizacao = StandViewModel.Localizacao,
                        Ativo = StandViewModel.Ativo
                    };

                    await standRepository.AddAsync(stand);

                    SetTempData(TipoNotificacao.Sucesso, "Stand cadastrado com sucesso.");

                    return RedirectToPage("/Admin/GerenciamentoStand");
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o stand: {ex.Message}");
                return Page();
            }
        }

        private void ValidateOnPost()
        {

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
