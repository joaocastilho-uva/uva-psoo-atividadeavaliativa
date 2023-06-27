using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories;
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
            Produto = await produtoRepository.GetAsync(produtoId);
        }

        public async Task<IActionResult> OnPostReservar(Guid produtoId)
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    var produto = await produtoRepository.GetAsync(produtoId);

                    if (produto != null && produto.Id != Guid.Empty)
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

                                if (carrinho.ItensCarrinho.Any())
                                {
                                    var itemCarrinhoId = carrinho.ItensCarrinho.Where(w => w.ProdutoId == produtoId).Select(s => s.Id).FirstOrDefault();

                                    if (itemCarrinhoId != Guid.Empty)
                                    {
                                        itemCarrinho = await itemCarrinhoRepository.GetAsync(itemCarrinhoId);
                                        itemCarrinho.Quantidade += 1;

                                        await itemCarrinhoRepository.UpdateAsync(itemCarrinho);
                                        carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                                    }
                                }
                                else
                                {
                                    itemCarrinho.CarrinhoId = carrinho.Id;
                                    itemCarrinho.ProdutoId = produto.Id;
                                    itemCarrinho.ImagemUrl = produto.ImagemUrl;
                                    itemCarrinho.Quantidade = 1;
                                    itemCarrinho.ValorReserva = (produto.ValorAtual * 0.15m);

                                    carrinho.ItensCarrinho.Add(itemCarrinho);
                                    carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                                    await carrinhoRepository.UpdateAsync(carrinho);
                                }
                            }
                        }
                        else
                        {
                            var itemCarrinho = new ItemCarrinho()
                            {
                                CarrinhoId = carrinho.Id,
                                ProdutoId = produto.Id,
                                ImagemUrl = produto.ImagemUrl,
                                Quantidade = 1,
                                ValorReserva = (produto.ValorAtual * 0.15m)
                            };

                            carrinho.ItensCarrinho.Add(itemCarrinho);
                            await carrinhoRepository.UpdateAsync(carrinho);
                        }
                    }
                }

                return Redirect($"/Default/Catalogo/{(int)Categoria}");
            }
            catch (Exception ex)
            {
                SetViewData(TipoNotificacao.Erro, $"Não foi possível incluir o produto no carrinho: {ex.Message}");
                return Redirect($"/Default/Catalogo/{(int)Categoria}");
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
