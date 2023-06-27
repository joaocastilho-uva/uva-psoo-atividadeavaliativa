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
    public class GerenciamentoStandModel : PageModel
    {
        private readonly IStandRepository standRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly UserManager<IdentityUser> userManager;

        public List<Stand> Stands { get; set; }

        [BindProperty]
        public UsuarioViewModel UsuarioViewModel { get; set; }

        public GerenciamentoStandModel(IStandRepository standRepository,
            IUsuarioRepository usuarioRepository,
            UserManager<IdentityUser> userManager)
        {
            this.standRepository = standRepository;
            this.usuarioRepository = usuarioRepository;
            this.userManager = userManager;
        }

        public async Task OnGet()
        {
            GetTempData();

            var usuario = await userManager.GetUserAsync(User);

            if (usuario != null)
            {
                if (await userManager.IsInRoleAsync(usuario, "SuperAdmin"))
                {
                    Stands = (await standRepository.GetAllAsync()).ToList();
                }
                else
                {
                    Stands = (await standRepository.GetAllAsync(await userManager.GetUserAsync(User))).ToList();
                }

                if (!Stands.Any())
                {
                    SetViewData(TipoNotificacao.Informativa, "Não foram encontrados stands cadastrados.");
                }
            }
        }

        public async Task OnPostDelete()
        {
            try
            {
                ValidateOnPostDelete();


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task OnPostAtivarDesativar(Guid id)
        {
            try
            {
                if (Stands.Any())
                {
                    var stand = Stands.FirstOrDefault(w => w.Id == id);

                    if (stand != null && !stand.Ativo)
                    {
                    }
                    else
                    {
                    }

                    if (ModelState.IsValid)
                    {

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateOnPost()
        {

        }

        private void ValidateOnPostDelete()
        {

        }

        private void ValidateOnPostAtivarDesativar(Stand stand)
        {

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
