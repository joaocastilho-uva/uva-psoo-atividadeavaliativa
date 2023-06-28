using ArteConexao.Models;
using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IItemReservaRepository
    {
        Task<ItemReserva> GetAsync(Guid id);
        Task<IEnumerable<ItemReserva>> GetAllAsync(IdentityUser identityUser);
    }
}
