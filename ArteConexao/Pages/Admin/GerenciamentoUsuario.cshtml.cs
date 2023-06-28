using ArteConexao.Enums;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient.Server;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class GerenciamentoUsuarioModel : PageModel
    {
        private readonly IStandRepository standRepository;
        private readonly IUsuarioRepository usuarioRepository;

        public List<IdentityUser> Usuarios { get; set; }

        [BindProperty]
        public Guid StandId { get; set; }

        public GerenciamentoUsuarioModel(IStandRepository standRepository,
            IUsuarioRepository usuarioRepository)
        {
            this.standRepository = standRepository;
            this.usuarioRepository = usuarioRepository;

            Usuarios = new List<IdentityUser>();
        }

        public async Task OnGet(Guid standId)
        {
            GetTempData();

            if (standId != Guid.Empty)
            {
                StandId = standId;
                var stand = await standRepository.GetAsync(standId);

                if (stand != null)
                {
                    if (stand.Usuarios.Any())
                    {
                        Usuarios = stand.Usuarios.ToList();
                    }
                    else
                    {
                        SetViewData(TipoNotificacao.Informativa, "Não foram encontrados usuários cadastrados.");
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            try
            {
                ValidateOnPostDelete();

                if (id != Guid.Empty)
                {
                    var deletado = await usuarioRepository.DeleteAsync(id);

                    if (deletado)
                    {
                        var stand = await standRepository.GetAsync(StandId);

                        if (stand != null)
                        {
                            stand.Usuarios = stand.Usuarios.Where(w => w.Id != id.ToString()).ToList();
                            await standRepository.UpdateAsync(stand);
                        }

                        var notificacao = new NotificacaoViewModel
                        {
                            Tipo = TipoNotificacao.Sucesso,
                            Mensagem = "Usuário excluído com sucesso."
                        };

                        TempData["Notificacao"] = JsonSerializer.Serialize(notificacao);
                    }
                }

                return Redirect($"/admin/gerenciamentousuario/{StandId}");
            }
            catch (Exception ex)
            {
                SetTempData(TipoNotificacao.Erro, $"Não foi possível excluir o usuário: {ex.Message}.");

                return Redirect($"/admin/gerenciamentousuario/{StandId}");
            }
        }

        private void ValidateOnPostDelete()
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
