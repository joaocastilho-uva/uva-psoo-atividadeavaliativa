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

        [BindProperty]
        public string Categoria { get; set; }

        [BindProperty]
        public string PaisOrigem { get; set; }

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
                    ValorAtual = produto.ValorAtual,
                    StandId = produto.StandId,
                    QuantidadeDisponivel = produto.QuantidadeDisponivel,
                    Comprimento = produto.Comprimento,
                    Largura = produto.Largura,
                    Altura = produto.Altura,
                    UsuarioInclusao = produto.UsuarioInclusao,
                    DataInclusao = produto.DataInclusao,
                    UsuarioAlteracao = produto.UsuarioAlteracao,
                    DataAlteracao = produto.DataAlteracao
                };

                StandId = produto.StandId;
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
                        PaisOrigem = Enum.GetValues(typeof(Pais)).Cast<Pais>().FirstOrDefault(v => v.ObterDescricao() == PaisOrigem),
                        Categoria = Enum.GetValues(typeof(Categoria)).Cast<Categoria>().FirstOrDefault(v => v.ObterDescricao() == Categoria),
                        Comprimento = ProdutoViewModel.Comprimento,
                        Largura = ProdutoViewModel.Largura,
                        Altura = ProdutoViewModel.Altura,
                        QuantidadeDisponivel = ProdutoViewModel.QuantidadeDisponivel,
                        ValorAtual = ProdutoViewModel.ValorAtual,
                        UsuarioAlteracao = new Guid(userManager.GetUserId(User)),
                        DataAlteracao = DateTime.Now
                    };

                    await produtoRepository.UpdateAsync(produto);

                    SetTempData(TipoNotificacao.Sucesso, "Produto atualizado com sucesso.");

                    return Redirect($"/Admin/GerenciamentoProduto/{ProdutoViewModel.StandId}");
                }
                else
                {
                    SetViewData(TipoNotificacao.Erro, $"N�o foi poss�vel cadastrar o produto: <br />{string.Join("<br />", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))}");
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"N�o foi poss�vel atualizar o produto: {ex.Message}.");

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
