using ArteConexao.Models;
using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IItemReservaRepository
    {
        Task<ItemReserva> GetAsync(Guid itemReservaId);
        Task<IEnumerable<ItemReserva>> GetAllAsync(Guid standId);
        Task<ItemReserva> UpdateAsync(ItemReserva itemReserva);
    }
}
