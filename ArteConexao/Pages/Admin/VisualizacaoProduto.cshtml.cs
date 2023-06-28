using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace ArteConexao.Pages.Admin
{
    public class VisualizacaoProdutoModel : PageModel
    {
        private readonly IProdutoRepository produtoRepository;

        [BindProperty]
        public ProdutoViewModel ProdutoViewModel { get; set; }

        public VisualizacaoProdutoModel(IProdutoRepository produtoRepository)
        {
            this.produtoRepository = produtoRepository;
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
                    Descricao = HttpUtility.HtmlDecode(produto.Descricao),
                    ImagemUrl = produto.ImagemUrl,
                    PaisOrigem = produto.PaisOrigem,
                    Categoria = produto.Categoria,
                    StandId = produto.StandId,
                    ValorAtual = produto.ValorAtual,
                    QuantidadeDisponivel = produto.QuantidadeDisponivel,
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
    }
}
