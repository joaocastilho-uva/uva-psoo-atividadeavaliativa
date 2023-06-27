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

        public List<Produto> Produtos { get; set; }
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
        }

        public async Task<IActionResult> OnGet(Categoria categoria)
        {
            Categoria = categoria;
            Produtos = (await produtoRepository.GetAllAsync(categoria)).ToList();

            if (!Produtos.Any())
            {
                SetViewData(TipoNotificacao.Erro, "Nenhum produto encontrado.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostReserva(Guid produtoId)
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
                                    itemCarrinho.StandId = produto.StandId;
                                    itemCarrinho.Quantidade = 1;
                                    itemCarrinho.ImagemUrl = produto.ImagemUrl;
                                    itemCarrinho.ValorReserva = (produto.ValorAtual * 0.15m);

                                    await AtualizarCarrinho(carrinho, itemCarrinho, (itemCarrinho.ValorReserva * itemCarrinho.Quantidade));
                                    //carrinho.ItensCarrinho.Add(itemCarrinho);
                                    //carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                                    //await carrinhoRepository.UpdateAsync(carrinho);
                                }
                            }
                        }
                        else
                        {
                            var itemCarrinho = new ItemCarrinho()
                            {
                                CarrinhoId = carrinho.Id,
                                ProdutoId = produto.Id,
                                StandId = produto.StandId,
                                Quantidade = 1,
                                ImagemUrl = produto.ImagemUrl,
                                ValorReserva = (produto.ValorAtual * 0.15m)
                            };

                            await AtualizarCarrinho(carrinho, itemCarrinho, (itemCarrinho.ValorReserva * itemCarrinho.Quantidade));

                            //carrinho.ItensCarrinho.Add(itemCarrinho);
                            //carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
                            //await carrinhoRepository.UpdateAsync(carrinho);
                            //await AtualizarQuantidadeDisponivel(produto, itemCarrinho.Quantidade);
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

        private async Task AtualizarCarrinho(Carrinho carrinho, ItemCarrinho itemCarrinho, decimal valorTotal)
        {
            carrinho.ItensCarrinho.Add(itemCarrinho);
            carrinho.ValorTotal += (itemCarrinho.ValorReserva * itemCarrinho.Quantidade);
            await carrinhoRepository.UpdateAsync(carrinho);
        }

        private async Task AtualizarQuantidadeDisponivel(Produto produto, int quantidadeReservada)
        {
            produto.QuantidadeDisponivel -= quantidadeReservada;
            await produtoRepository.UpdateAsync(produto);
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
