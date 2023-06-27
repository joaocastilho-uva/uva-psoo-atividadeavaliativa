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

        public Task<Reserva> AddAsync(Reserva reserva)
        {
            throw new NotImplementedException();
        }

        public Task<Reserva> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<Reserva>> GetAllAsync(Stand stand)
        //{
        //    return await arteConexaoDbContext.Reservas.Where(w => w.ItensReserva.SelectMany(s => s.StandId == stand.Id)).ToListAsync();
        //}

        public async Task<IEnumerable<Reserva>> GetAllAsync(IdentityUser identityUser)
        {
            return await arteConexaoDbContext.Reservas.Include(nameof(Reserva.ItensReserva)).Where(w => w.UsuarioId == new Guid(identityUser.Id)).ToListAsync();
        }

        public Task<Reserva> UpdateAsync(Reserva stand)
        {
            throw new NotImplementedException();
        }
    }
}
