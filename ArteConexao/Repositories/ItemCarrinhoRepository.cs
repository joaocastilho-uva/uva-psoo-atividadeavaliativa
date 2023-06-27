using ArteConexao.Data;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class ItemCarrinhoRepository : IItemCarrinhoRepository
    {
        private readonly ArteConexaoDbContext arteConexaoDbContext;

        public ItemCarrinhoRepository(ArteConexaoDbContext arteConexaoDbContext)
        {
            this.arteConexaoDbContext = arteConexaoDbContext;
        }

        public async Task<ItemCarrinho> GetAsync(Guid itemCarrinhoId)
        {
            return await arteConexaoDbContext.ItensCarrinho.FirstOrDefaultAsync(f => f.Id == itemCarrinhoId);
        }

        public async Task<ItemCarrinho> UpdateAsync(ItemCarrinho itemCarrinho)
        {
            if (itemCarrinho != null)
            {
                var itemCarrinhoDb = await arteConexaoDbContext.ItensCarrinho.FirstOrDefaultAsync(f => f.Id == itemCarrinho.Id);

                if (itemCarrinhoDb != null)
                {
                    itemCarrinhoDb.Quantidade = itemCarrinho.Quantidade;

                    await arteConexaoDbContext.SaveChangesAsync();
                }
            }

            return itemCarrinho;
        }

        public async Task<bool> DeleteAsync(Guid itemCarrinhoId)
        {
            if (itemCarrinhoId != Guid.Empty)
            {
                var itemCarrinhoDb = await arteConexaoDbContext.ItensCarrinho.FirstOrDefaultAsync(x => x.Id == itemCarrinhoId);

                if (itemCarrinhoDb != null)
                {
                    arteConexaoDbContext.ItensCarrinho.Remove(itemCarrinhoDb);
                    await arteConexaoDbContext.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
    }
}
