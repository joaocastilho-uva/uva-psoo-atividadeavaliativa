using ArteConexao.Enums;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Admin
{
    public class InclusaoUsuarioModel : PageModel
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IStandRepository standRepository;

        [BindProperty]
        public Guid StandId { get; set; }

        [BindProperty]
        public UsuarioViewModel UsuarioViewModel { get; set; }

        public InclusaoUsuarioModel(IUsuarioRepository usuarioRepository,
            IStandRepository standRepository)
        {
            this.usuarioRepository = usuarioRepository;
            this.standRepository = standRepository;
        }

        public async Task OnGet(Guid standId)
        {
            StandId = standId;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                ValidateOnPost();

                if (ModelState.IsValid)
                {
                    var identityUser = new IdentityUser()
                    {
                        UserName = UsuarioViewModel.Nome,
                        Email = UsuarioViewModel.Email
                    };

                    var roles = new List<string>() { "User" };

                    if (UsuarioViewModel.Admin)
                    {
                        roles.Add("Admin");
                    }

                    var identityResult = await usuarioRepository.AddAsync(identityUser, UsuarioViewModel.Senha, roles);

                    if (identityResult.Succeeded)
                    {
                        var stand = await standRepository.GetAsync(StandId);

                        if (stand != null)
                        {
                            stand.Usuarios.Add(identityUser);
                            await standRepository.UpdateAsync(stand);
                        }

                        SetTempData(TipoNotificacao.Sucesso, "Usuário cadastrado com sucesso.");
                        return Redirect($"/Admin/GerenciamentoUsuario/{StandId}");
                    }
                    else
                    {
                        SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o usuário: <br />{string.Join("<br />", identityResult.Errors.Select(x => x.Description))}");
                    }
                }
                else
                {
                    SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o usuário: <br />{string.Join("<br />", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))}");
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o usuário: {ex.Message}");

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
