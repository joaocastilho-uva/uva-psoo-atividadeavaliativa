using Microsoft.AspNetCore.Identity;

namespace ArteConexao.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IdentityResult> AddAsync(IdentityUser identityUser, string password, List<string> roles);
        Task<IdentityUser> GetAsync(Guid id);
        Task<IEnumerable<IdentityUser>> GetAllAsync();
        Task<bool> DeleteAsync(Guid usuarioId);
    }
}
