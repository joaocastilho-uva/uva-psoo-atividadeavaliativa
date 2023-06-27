using ArteConexao.Data;
using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AutorizacaoDbContext autorizacaoDbContext;
        private readonly UserManager<IdentityUser> userManager;

        public UsuarioRepository(AutorizacaoDbContext autorizacaoDbContext,
            UserManager<IdentityUser> userManager)
        {
            this.autorizacaoDbContext = autorizacaoDbContext;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddAsync(IdentityUser identityUser, string senha, List<string> roles)
        {
            var identityResult = await userManager.CreateAsync(identityUser, senha);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, roles);
            }

            return identityResult;
        }

        public async Task<IdentityUser> GetAsync(Guid id)
        {
            return await autorizacaoDbContext.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            var usuarios = await autorizacaoDbContext.Users.ToListAsync();
            var superAdminUser = await autorizacaoDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminUser != null)
            {
                usuarios.Remove(superAdminUser);
            }

            return usuarios;
        }

        public async Task<bool> DeleteAsync(Guid usuarioId)
        {
            var user = await userManager.FindByIdAsync(usuarioId.ToString());

            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return true;
            }

            return false;
        }
    }
}
