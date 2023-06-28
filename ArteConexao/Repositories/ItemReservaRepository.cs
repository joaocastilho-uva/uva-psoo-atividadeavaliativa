using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Repositories
{
    public class ItemReservaRepository : IItemReservaRepository
    {
        public Task<IEnumerable<ItemReserva>> GetAllAsync(IdentityUser identityUser)
        {
            throw new NotImplementedException();
        }

        public Task<ItemReserva> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
