using ArteConexao.Data;
using ArteConexao.Enums;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ArteConexaoDbContext arteConexaoDbContext;

        public ProdutoRepository(ArteConexaoDbContext arteConexaoDbContext)
        {
            this.arteConexaoDbContext = arteConexaoDbContext;
        }

        public async Task<Produto> AddAsync(Produto produto)
        {
            await arteConexaoDbContext.Produtos.AddAsync(produto);
            await arteConexaoDbContext.SaveChangesAsync();

            return produto;
        }

        public async Task<Produto> GetAsync(Guid id)
        {
            return await arteConexaoDbContext.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await arteConexaoDbContext.Produtos.Where(w => w.QuantidadeDisponivel > 0).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAllAsync(Categoria categoria)
        {
            if ((int)categoria == 1)
            {
                return await arteConexaoDbContext.Produtos.Where(w => w.QuantidadeDisponivel > 0).ToListAsync();
            }

            return await arteConexaoDbContext.Produtos.Where(w => w.Categoria == categoria && w.QuantidadeDisponivel > 0).ToListAsync();
        }

        public async Task<Produto> UpdateAsync(Produto produto)
        {
            if (produto != null)
            {
                var produtoDb = await arteConexaoDbContext.Produtos.FirstOrDefaultAsync(x => x.Id == produto.Id);

                if (produtoDb != null)
                {
                    produtoDb.Id = produto.Id;
                    produtoDb.Nome = produto.Nome;
                    produtoDb.Descricao = produto.Descricao;
                    produtoDb.ImagemUrl = produto.ImagemUrl;
                    produtoDb.Categoria = produto.Categoria;
                    produtoDb.PaisOrigem = produto.PaisOrigem;
                    produtoDb.ValorTotal = produto.ValorTotal;
                    produtoDb.QuantidadeTotal = produto.QuantidadeTotal;
                    produtoDb.QuantidadeDisponivel = produto.QuantidadeDisponivel;
                    produtoDb.Comprimento = produto.Comprimento;
                    produtoDb.Largura = produto.Largura;
                    produtoDb.Altura = produto.Altura;
                    produtoDb.UsuarioAlteracao = produto.UsuarioAlteracao;
                    produtoDb.DataAlteracao = produto.DataAlteracao;

                    await arteConexaoDbContext.SaveChangesAsync();
                }
            }

            return produto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                var produtoDb = await arteConexaoDbContext.Produtos.FirstOrDefaultAsync(x => x.Id == id);

                if (produtoDb != null)
                {
                    arteConexaoDbContext.Produtos.Remove(produtoDb);
                    await arteConexaoDbContext.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<int> ObterQuantidadeDisponivelAsync(Guid produtoId)
        {
            return arteConexaoDbContext.Produtos.FirstOrDefaultAsync(x => x.Id == produtoId).Result.QuantidadeDisponivel;
        }
    }
}
