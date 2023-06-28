using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using ArteConexao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ArteConexao.Pages.Default
{
    public class CatalogoModel : PageModel
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly ICarrinhoRepository carrinhoRepository;
        private readonly IItemCarrinhoRepository itemCarrinhoRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public List<ItemCatalogoViewModel> ItensCatalogoViewModel { get; set; }

        [BindProperty]
        public Categoria Categoria { get; set; }

        public CatalogoModel(IProdutoRepository produtoRepository,
            ICarrinhoRepository carrinhoRepository,
            IItemCarrinhoRepository itemCarrinhoRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.produtoRepository = produtoRepository;
            this.carrinhoRepository = carrinhoRepository;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;

            ItensCatalogoViewModel = new List<ItemCatalogoViewModel>();
        }

        public async Task OnGet(Categoria categoria)
        {
            Categoria = categoria;

            var produtosDb = (await produtoRepository.GetAllAsync(categoria)).ToList();

            if (produtosDb.Any())
            {
                foreach (var produtoDb in produtosDb)
                {
                    ItensCatalogoViewModel.Add(
                        new ItemCatalogoViewModel()
                        {
                            ProdutoId = produtoDb.Id,
                            Nome = produtoDb.Nome,
                            Descricao = produtoDb.Descricao,
                            ImagemUrl = produtoDb.ImagemUrl,
                            PaisOrigem = produtoDb.PaisOrigem,
                            QuantidadeDisponivel = produtoDb.QuantidadeDisponivel,
                            Comprimento = produtoDb.Comprimento,
                            Largura = produtoDb.Largura,
                            StandId = produtoDb.StandId,
                            Altura = produtoDb.Altura,
                            ValorTotal = produtoDb.ValorTotal,
                            ValorAtual = produtoDb.ValorAtual,
                            ValorReserva = produtoDb.ValorReserva
                        });
                }
            }
            else
            {
                SetViewData(TipoNotificacao.Erro, "Nenhum produto encontrado.");
            }
        }

        public async Task<IActionResult> OnPostReserva(Guid produtoId)
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    var itemCatalogoViewModel = ItensCatalogoViewModel.FirstOrDefault(w => w.ProdutoId == produtoId);

                    if (itemCatalogoViewModel != null
                        && itemCatalogoViewModel.ProdutoId != Guid.Empty
                        && itemCatalogoViewModel.QuantidadeDisponivel > 0)
                    {
                        var usuarioId = new Guid(userManager.GetUserId(User));
                        var carrinho = await carrinhoRepository.GetAsync(usuarioId);

                        if (carrinho == null)
                        {
                            carrinho = new Carrinho()
                            {
                                UsuarioId = usuarioId
                            };

                            await carrinhoRepository.AddAsync(carrinho);

                            if (carrinho != null && carrinho.Id != Guid.Empty)
                            {
                                var itemCarrinho = new ItemCarrinho();

                                itemCarrinho.CarrinhoId = carrinho.Id;
                                itemCarrinho.StandId = itemCatalogoViewModel.StandId;
                                itemCarrinho.ProdutoId = itemCatalogoViewModel.ProdutoId;
                                itemCarrinho.ImagemUrl = itemCatalogoViewModel.ImagemUrl;
                                itemCarrinho.Quantidade = 1;
                                itemCarrinho.ValorTotal = itemCatalogoViewModel.ValorAtual;
                                itemCarrinho.ValorReserva = itemCatalogoViewModel.ValorReserva;

                                carrinho.ItensCarrinho.Add(itemCarrinho);
                                carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                                await carrinhoRepository.UpdateAsync(carrinho);

                                SetViewData(TipoNotificacao.Informativa, "Produto adicionado ao carrinho com sucesso.");
                            }
                        }
                        else
                        {
                            var itemCarrinho = new ItemCarrinho();

                            if (carrinho.ItensCarrinho.Any())
                            {
                                var itemCarrinhoId = carrinho.ItensCarrinho.Where(w => w.ProdutoId == itemCatalogoViewModel.ProdutoId).Select(s => s.Id).FirstOrDefault();

                                if (itemCarrinhoId != Guid.Empty)
                                {
                                    SetViewData(TipoNotificacao.Informativa, "Produto já adicionado ao carrinho.");
                                }
                            }
                            else
                            {
                                itemCarrinho.CarrinhoId = carrinho.Id;
                                itemCarrinho.StandId = itemCatalogoViewModel.StandId;
                                itemCarrinho.ProdutoId = itemCatalogoViewModel.ProdutoId;
                                itemCarrinho.ImagemUrl = itemCatalogoViewModel.ImagemUrl;
                                itemCarrinho.Quantidade = 1;
                                itemCarrinho.ValorTotal = itemCatalogoViewModel.ValorAtual;
                                itemCarrinho.ValorReserva = itemCatalogoViewModel.ValorReserva;

                                carrinho.ItensCarrinho.Add(itemCarrinho);
                                carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                                await carrinhoRepository.UpdateAsync(carrinho);

                                SetViewData(TipoNotificacao.Informativa, "Produto adicionado ao carrinho com sucesso.");
                            }
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível incluir o produto no carrinho: {ex.Message}");
                return Page();
            }
        }

        private async Task AtualizarCarrinho(Carrinho carrinho, ItemCarrinho itemCarrinho, decimal valorTotal)
        {
            carrinho.ItensCarrinho.Add(itemCarrinho);
            carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
            await carrinhoRepository.UpdateAsync(carrinho);
        }

        //private async Task AtualizarQuantidadeDisponivel(Produto produto, int quantidadeReservada)
        //{
        //    produto.QuantidadeDisponivel -= quantidadeReservada;
        //    await produtoRepository.UpdateAsync(produto);
        //}

        private void SetViewData(TipoNotificacao tipoNotificacao, string mensagem)
        {
            var notificacao = new NotificacaoViewModel
            {
                Tipo = tipoNotificacao,
                Mensagem = mensagem
            };

            ViewData["Notificacao"] = notificacao;
        }

        private void GetTempData()
        {
            var notificaticao = (string)TempData["Notificacao"];

            if (!string.IsNullOrWhiteSpace(notificaticao))
            {
                ViewData["Notificacao"] = JsonSerializer.Deserialize<NotificacaoViewModel>(notificaticao.ToString()); ;
            }
        }
    }
}
