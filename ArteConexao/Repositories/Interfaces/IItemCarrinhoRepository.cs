using ArteConexao.Models;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IItemCarrinhoRepository
    {
        Task<ItemCarrinho> GetAsync(Guid itemCarrinhoId);
        Task<ItemCarrinho> UpdateAsync(ItemCarrinho itemCarrinho);
        Task<bool> DeleteAsync(Guid itemCarrinhoId);
    }
}
