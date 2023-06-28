using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArteConexao.Pages.User
{
    public class FinalizacaoReservaModel : PageModel
    {
        private readonly IReservaRepository reservaRepository;
        private readonly IProdutoRepository produtoRepository;

        public FinalizacaoReservaModel(IReservaRepository reservaRepository,
            IProdutoRepository produtoRepository)
        {
            this.reservaRepository = reservaRepository;
            this.produtoRepository = produtoRepository;
        }

        public async Task OnGet(Guid reservaId)
        {
            try
            {
                var reserva = await reservaRepository.GetAsync(reservaId);

                if (reserva != null)
                {
                    if (reserva.ItensReserva.Any())
                    {
                        foreach (var itemReserva in reserva.ItensReserva)
                        {
                            var produtoDb = await produtoRepository.GetAsync(itemReserva.ProdutoId);

                            if (produtoDb != null)
                            {
                                produtoDb.QuantidadeDisponivel -= itemReserva.Quantidade;
                                await produtoRepository.UpdateAsync(produtoDb);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
