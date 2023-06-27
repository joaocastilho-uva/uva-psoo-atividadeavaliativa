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
    public class ItemCatalogoModel : PageModel
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly ICarrinhoRepository carrinhoRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IItemCarrinhoRepository itemCarrinhoRepository;

        [BindProperty]
        public ItemCatalogoViewModel ItemCatalogoViewModel { get; set; }

        public Produto Produto { get; set; }

        [BindProperty]
        public Categoria Categoria { get; set; }

        public ItemCatalogoModel(IProdutoRepository produtoRepository,
            ICarrinhoRepository carrinhoRepository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IItemCarrinhoRepository itemCarrinhoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.carrinhoRepository = carrinhoRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.itemCarrinhoRepository = itemCarrinhoRepository;
        }

        public async Task OnGet(Guid produtoId)
        {
            try
            {
                var produtoDb = await produtoRepository.GetAsync(produtoId);

                if (produtoDb != null)
                {
                    ItemCatalogoViewModel = new ItemCatalogoViewModel()
                    {
                        ProdutoId = produtoDb.Id,
                        Nome = produtoDb.Nome,
                        Descricao = produtoDb.Descricao,
                        ImagemUrl = produtoDb.ImagemUrl,
                        PaisOrigem = produtoDb.PaisOrigem,
                        QuantidadeDisponivel = produtoDb.QuantidadeDisponivel,
                        Comprimento = produtoDb.Comprimento,
                        Largura = produtoDb.Largura,
                        Altura = produtoDb.Altura,
                        ValorAtual = produtoDb.ValorAtual,
                        ValorReserva = produtoDb.ValorReserva
                    };
                }
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível carregar o produto {ex.Message}.");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    if (ItemCatalogoViewModel != null
                        && ItemCatalogoViewModel.ProdutoId != Guid.Empty
                        && ItemCatalogoViewModel.QuantidadeDisponivel > 0)
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
                                itemCarrinho.ProdutoId = ItemCatalogoViewModel.ProdutoId;
                                itemCarrinho.ImagemUrl = ItemCatalogoViewModel.ImagemUrl;
                                itemCarrinho.ValorTotal = ItemCatalogoViewModel.ValorAtual;
                                itemCarrinho.Quantidade = ItemCatalogoViewModel.Quantidade;
                                itemCarrinho.ValorReserva = ItemCatalogoViewModel.ValorReserva;

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
                                var itemCarrinhoId = carrinho.ItensCarrinho.Where(w => w.ProdutoId == ItemCatalogoViewModel.ProdutoId).Select(s => s.Id).FirstOrDefault();

                                if (itemCarrinhoId != Guid.Empty)
                                {
                                    SetViewData(TipoNotificacao.Informativa, "Produto já adicionado ao carrinho.");
                                }
                            }
                            else
                            {
                                itemCarrinho.CarrinhoId = carrinho.Id;
                                itemCarrinho.ProdutoId = ItemCatalogoViewModel.ProdutoId;
                                itemCarrinho.ImagemUrl = ItemCatalogoViewModel.ImagemUrl;
                                itemCarrinho.ValorTotal = ItemCatalogoViewModel.ValorAtual;
                                itemCarrinho.Quantidade = ItemCatalogoViewModel.Quantidade;
                                itemCarrinho.ValorReserva = ItemCatalogoViewModel.ValorReserva;

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
