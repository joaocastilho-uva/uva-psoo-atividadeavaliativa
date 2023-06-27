using ArteConexao.Enums;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArteConexao.Pages.Default
{
    public class CadastroModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public CadastroViewModel CadastroViewModel { get; set; }

        public CadastroModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                ValidateOnPost();

                if (ModelState.IsValid)
                {
                    var user = new IdentityUser()
                    {
                        UserName = CadastroViewModel.Nome,
                        Email = CadastroViewModel.Email
                    };

                    var identityResult = await userManager.CreateAsync(user, CadastroViewModel.Senha);

                    if (identityResult.Succeeded)
                    {
                        var addRolesResult = await userManager.AddToRoleAsync(user, "User");

                        if (addRolesResult.Succeeded)
                        {
                            SetViewData(TipoNotificacao.Sucesso, "Usuário cadastrado com sucesso.");
                            return RedirectToPage("../Index");
                        }
                    }

                    SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o usuário: <br />{string.Join("<br />", identityResult.Errors.Select(x => x.Description))}");
                    return Page();
                }
                else
                {
                    SetViewData(TipoNotificacao.Erro, $"Não foi possível cadastrar o usuário: <br />{string.Join("<br />", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))}");
                    return Page();
                }
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
