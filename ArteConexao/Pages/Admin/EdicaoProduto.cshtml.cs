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
    public class EdicaoProdutoModel : PageModel
    {
        private readonly IProdutoRepository produtoRepository;
        public readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public ProdutoViewModel ProdutoViewModel { get; set; }

        [BindProperty]
        public Guid StandId { get; set; }

        public EdicaoProdutoModel(IProdutoRepository produtoRepository,
            UserManager<IdentityUser> userManager)
        {
            this.produtoRepository = produtoRepository;
            this.userManager = userManager;
        }

        public async Task OnGet(Guid produtoId)
        {
            var produto = await produtoRepository.GetAsync(produtoId);

            if (produto != null)
            {
                ProdutoViewModel = new ProdutoViewModel()
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    ImagemUrl = produto.ImagemUrl,
                    Categoria = produto.Categoria,
                    PaisOrigem = produto.PaisOrigem,
                    ValorTotal = produto.ValorAtual,
                    StandId = produto.StandId,
                    QuantidadeTotal = produto.QuantidadeTotal,
                    Comprimento = produto.Comprimento,
                    Largura = produto.Largura,
                    Altura = produto.Altura,
                    UsuarioInclusao = produto.UsuarioInclusao,
                    DataInclusao = produto.DataInclusao,
                    UsuarioAlteracao = produto.UsuarioAlteracao,
                    DataAlteracao = produto.DataAlteracao
                };
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                ValidateOnPost();

                if (ModelState.IsValid)
                {
                    var produto = new Produto()
                    {
                        Id = ProdutoViewModel.Id,
                        Nome = ProdutoViewModel.Nome,
                        Descricao = ProdutoViewModel.Descricao,
                        ImagemUrl = ProdutoViewModel.ImagemUrl,
                        PaisOrigem = ProdutoViewModel.PaisOrigem,
                        Categoria = ProdutoViewModel.Categoria,
                        Comprimento = ProdutoViewModel.Comprimento,
                        Largura = ProdutoViewModel.Largura,
                        Altura = ProdutoViewModel.Altura,
                        QuantidadeTotal = ProdutoViewModel.QuantidadeTotal,
                        ValorAtual = ProdutoViewModel.ValorTotal,
                        UsuarioAlteracao = new Guid(userManager.GetUserId(User)),
                        DataAlteracao = DateTime.Now
                    };

                    await produtoRepository.UpdateAsync(produto);

                    SetTempData(TipoNotificacao.Sucesso, "Produto atualizado com sucesso.");

                    return Redirect($"/admin/gerenciamentoproduto/{ProdutoViewModel.StandId}");
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível atualizar o produto: {ex.Message}.");

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
