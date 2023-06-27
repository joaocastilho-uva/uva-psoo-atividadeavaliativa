using ArteConexao.Enums;
using ArteConexao.Models;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> AddAsync(Produto produto);
        //Task<IEnumerable<Produto>> AddAllAsync();
        Task<Produto> GetAsync(Guid id);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<IEnumerable<Produto>> GetAllAsync(Categoria categoria);
        Task<Produto> UpdateAsync(Produto produto);
        Task<bool> DeleteAsync(Guid id);
    }
}
