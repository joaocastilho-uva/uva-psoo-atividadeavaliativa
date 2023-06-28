using ArteConexao.Data;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class ItemReservaRepository : IItemReservaRepository
    {
        private readonly ArteConexaoDbContext arteConexaoDbContext;

        public ItemReservaRepository(ArteConexaoDbContext arteConexaoDbContext)
        {
            this.arteConexaoDbContext = arteConexaoDbContext;
        }

        public async Task<IEnumerable<ItemReserva>> GetAllAsync(Guid standId)
        {
            return await arteConexaoDbContext.ItensReserva.Where(w => w.StandId == standId).ToListAsync();
        }

        public async Task<ItemReserva> GetAsync(Guid itemReservaId)
        {
            return await arteConexaoDbContext.ItensReserva.FirstOrDefaultAsync(w => w.Id == itemReservaId);
        }

        public async Task<ItemReserva> UpdateAsync(ItemReserva itemReserva)
        {
            if (itemReserva != null)
            {
                var itemReservaDb = await arteConexaoDbContext.ItensReserva.FirstOrDefaultAsync(f => f.Id == itemReserva.Id);

                if (itemReservaDb != null)
                {
                    itemReservaDb.Status = itemReserva.Status;

                    await arteConexaoDbContext.SaveChangesAsync();
                }
            }

            return itemReserva;
        }
    }
}
