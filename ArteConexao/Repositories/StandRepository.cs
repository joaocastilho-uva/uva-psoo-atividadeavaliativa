using ArteConexao.Data;
using ArteConexao.Models;
using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class StandRepository : IStandRepository
    {
        private readonly ArteConexaoDbContext arteConexaoDbContext;
        private readonly AutorizacaoDbContext autorizacaoDbContext;

        public StandRepository(ArteConexaoDbContext arteConexaoDbContext,
            AutorizacaoDbContext autorizacaoDbContext)
        {
            this.arteConexaoDbContext = arteConexaoDbContext;
            this.autorizacaoDbContext = autorizacaoDbContext;
        }

        public async Task<Stand> AddAsync(Stand stand)
        {
            await arteConexaoDbContext.Stands.AddAsync(stand);
            await arteConexaoDbContext.SaveChangesAsync();

            return stand;
        }

        public async Task<Stand> GetAsync(Guid id)
        {
            return await arteConexaoDbContext.Stands
                        .Include(nameof(Stand.Produtos))
                        .Include(nameof(Stand.ItensReserva))
                        .Include(nameof(Stand.Usuarios))
                        .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Stand>> GetAllAsync()
        {
            return await arteConexaoDbContext.Stands.ToListAsync();
        }

        public async Task<IEnumerable<Stand>> GetAllAsync(IdentityUser identityUser)
        {
            return await arteConexaoDbContext.Stands.Where(W => W.Usuarios.Contains(identityUser)).ToListAsync();
        }

        public async Task<Stand> UpdateAsync(Stand stand)
        {
            if (stand != null)
            {
                var standDb = await arteConexaoDbContext.Stands.FirstOrDefaultAsync(f => f.Id == stand.Id);

                if (standDb != null)
                {
                    standDb.Nome = stand.Nome;
                    standDb.Localizacao = stand.Localizacao;

                    await arteConexaoDbContext.SaveChangesAsync();
                }
            }

            return stand;
        }
    }
}
