using ArteConexao.Data;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ArteConexaoDbContext arteConexaoDbContext;

        public ReservaRepository(ArteConexaoDbContext arteConexaoDbContext)
        {
            this.arteConexaoDbContext = arteConexaoDbContext;
        }

        public async Task<Reserva> AddAsync(Reserva reserva)
        {
            await arteConexaoDbContext.Reservas.AddAsync(reserva);
            await arteConexaoDbContext.SaveChangesAsync();
            return reserva;
        }

        public async Task<Reserva> GetAsync(Guid reservaId)
        {
            return await arteConexaoDbContext.Reservas.Include(nameof(Reserva.ItensReserva)).FirstOrDefaultAsync(x => x.Id == reservaId);
        }

        //public async Task<IEnumerable<Reserva>> GetAllAsync(Stand stand)
        //{
        //    return await arteConexaoDbContext.Reservas.Where(w => w.ItensReserva.SelectMany(s => s.StandId == stand.Id)).ToListAsync();
        //}

        public async Task<IEnumerable<Reserva>> GetAllAsync(IdentityUser identityUser)
        {
            return await arteConexaoDbContext.Reservas.Include(nameof(Reserva.ItensReserva)).Where(w => w.UsuarioId == new Guid(identityUser.Id)).ToListAsync();
        }

        public async Task<Reserva> UpdateAsync(Reserva reserva)
        {
            var reservaDb = await arteConexaoDbContext.Reservas.FirstOrDefaultAsync(x => x.Id == reserva.Id);

            if (reservaDb == null)
            {
                if (reservaDb.ItensReserva.Any())
                {
                    arteConexaoDbContext.ItensReserva.RemoveRange(reservaDb.ItensReserva);

                    reservaDb.ItensReserva.ToList().ForEach(x => x.ReservaId = reservaDb.Id);
                    await arteConexaoDbContext.ItensReserva.AddRangeAsync(reservaDb.ItensReserva);
                }
            }

            await arteConexaoDbContext.SaveChangesAsync();
            return reservaDb;
        }
    }
}
