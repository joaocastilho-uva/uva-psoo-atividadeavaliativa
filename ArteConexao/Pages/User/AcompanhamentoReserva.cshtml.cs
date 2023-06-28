using ArteConexao.Models;
using ArteConexao.Repositories;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArteConexao.Pages.User
{
    public class AcompanhamentoReservaModel : PageModel
    {
        private readonly IReservaRepository reservaRepository;
        private readonly IProdutoRepository produtoRepository;
        private readonly UserManager<IdentityUser> userManager;

        public List<ItemReservaViewModel> ItensReservaViewModel { get; set; }

        public AcompanhamentoReservaModel(IReservaRepository reservaRepository,
            IProdutoRepository produtoRepository,
            UserManager<IdentityUser> userManager)
        {
            this.reservaRepository = reservaRepository;
            this.produtoRepository = produtoRepository;
            this.userManager = userManager;

            ItensReservaViewModel = new List<ItemReservaViewModel>();
        }

        public async Task OnGet(Guid usuarioId)
        {
            var reservas = await reservaRepository.GetAllAsync(await userManager.GetUserAsync(User));

            if (reservas.Any())
            {
                var itensReserva = reservas.SelectMany(s => s.ItensReserva);

                if (itensReserva.Any())
                {
                    foreach (var itemReserva in itensReserva)
                    {
                        var produtoDb = await produtoRepository.GetAsync(itemReserva.ProdutoId);

                        ItensReservaViewModel.Add(new ItemReservaViewModel()
                        {
                            Id = itemReserva.Id,
                            Nome = produtoDb.Nome,
                            ProdutoId = itemReserva.ProdutoId,
                            Status = itemReserva.Status,
                            Quantidade = itemReserva.Quantidade,
                            ValorReserva = itemReserva.ValorReserva,
                            ValorTotal = itemReserva.ValorTotal,
                        });
                    }
                }
            }
        }
    }
}
