using ArteConexao.Enums;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class EdicaoUsuarioModel : PageModel
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly UserManager<IdentityUser> userManager;

        public UsuarioViewModel UsuarioViewModel { get; set; }

        public Guid StandId { get; set; }

        public EdicaoUsuarioModel(IUsuarioRepository usuarioRepository,
            UserManager<IdentityUser> userManager)
        {
            this.usuarioRepository = usuarioRepository;
            this.userManager = userManager;
        }

        public async Task OnGet(Guid usuarioId)
        {
            if (usuarioId != Guid.Empty)
            {
                var usuarioDb = await usuarioRepository.GetAsync(usuarioId);

                if (usuarioDb != null)
                {
                    UsuarioViewModel = new UsuarioViewModel()
                    {
                        Id = new Guid(usuarioDb.Id),
                        Nome = usuarioDb.UserName,
                        Email = usuarioDb.Email,
                        Admin = await userManager.IsInRoleAsync(usuarioDb, "Admin")
                    };
                }
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
