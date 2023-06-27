using ArteConexao.Models;
using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IStandRepository
    {
        Task<Stand> AddAsync(Stand stand);
        Task<Stand> GetAsync(Guid id);
        Task<Stand> GetByUserAsync(IdentityUser usuario);
        Task<IEnumerable<Stand>> GetAllAsync();
        Task<IEnumerable<Stand>> GetAllAsync(IdentityUser identityUser);
        Task<Stand> UpdateAsync(Stand stand);
    }
}
