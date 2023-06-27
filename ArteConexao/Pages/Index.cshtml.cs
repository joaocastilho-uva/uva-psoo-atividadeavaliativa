using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArteConexao.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProdutoRepository _produtoRespository;

        public IndexModel(ILogger<IndexModel> logger,
            IProdutoRepository produtoRespository)
        {
            _logger = logger;
            _produtoRespository = produtoRespository;
        }

        public async Task OnGet()
        {

        }
    }
}