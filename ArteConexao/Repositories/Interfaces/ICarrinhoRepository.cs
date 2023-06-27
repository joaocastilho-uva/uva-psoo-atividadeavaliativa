using ArteConexao.Models;

namespace ArteConexao.Repositories.Interfaces
{
    public interface ICarrinhoRepository
    {
        Task<Carrinho> AddAsync(Carrinho carrinho);
        Task<Carrinho> GetAsync(Guid userId);
        Task<Carrinho> UpdateAsync(Carrinho carrinho);
        Task<bool> DeleteAsync(Guid carrinhoId);
    }
}
