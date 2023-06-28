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
    public class InclusaoProdutoModel : PageModel
    {
        private readonly IProdutoRepository produtoRepository;
        public readonly UserManager<IdentityUser> userManager;
        private readonly IStandRepository standRepository;

        [BindProperty]
        public Guid StandId { get; set; }

        public InclusaoProdutoModel(IProdutoRepository produtoRepository,
            UserManager<IdentityUser> userManager,
            IStandRepository standRepository)
        {
            this.produtoRepository = produtoRepository;
            this.userManager = userManager;
            this.standRepository = standRepository;
        }

        [BindProperty]
        public ProdutoViewModel ProdutoViewModel { get; set; }

        public void OnGet(Guid standId)
        {
            StandId = standId;
        }

        public async Task<IActionResult> OnPost()
        {
            ValidateOnPost();

            if (ModelState.IsValid)
            {
                var produto = new Produto()
                {
                    Nome = ProdutoViewModel.Nome,
                    Descricao = ProdutoViewModel.Descricao,
                    ImagemUrl = ProdutoViewModel.ImagemUrl,
                    PaisOrigem = ProdutoViewModel.PaisOrigem,
                    Categoria = ProdutoViewModel.Categoria,
                    ValorTotal = ProdutoViewModel.ValorTotal,
                    ValorAtual = ProdutoViewModel.ValorTotal,
                    ValorReserva = (ProdutoViewModel.ValorTotal * 0.10m),
                    QuantidadeTotal = ProdutoViewModel.QuantidadeTotal,
                    QuantidadeDisponivel = ProdutoViewModel.QuantidadeTotal,
                    Comprimento = ProdutoViewModel.Comprimento,
                    Largura = ProdutoViewModel.Largura,
                    Altura = ProdutoViewModel.Altura,
                    StandId = StandId,
                    UsuarioInclusao = new Guid(userManager.GetUserId(User)),
                    DataInclusao = DateTime.Now
                };

                await produtoRepository.AddAsync(produto);

                var stand = await standRepository.GetAsync(StandId);

                if (stand != null)
                {
                    stand.Produtos.Add(produto);
                    await standRepository.UpdateAsync(stand);
                }

                SetTempData(TipoNotificacao.Sucesso, "Produto cadastrado com sucesso.");
                return Redirect($"/Admin/GerenciamentoProduto/{StandId}");
            }

            return Page();
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
