using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArteConexao.Data
{
    public class AutorizacaoDbContext : IdentityDbContext
    {
        public AutorizacaoDbContext(DbContextOptions<AutorizacaoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var superAdminRoleId = "823fad49-1eef-4d2f-abab-fd79f3a49fc2";
            var adminRoleId = "c2141506-f5e2-4a6e-831e-d9842185de42";
            var userRoleId = "c214d14e-cb88-4568-802f-8e00cb3918c5";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var superAdminId = "134e2eb7-42cc-489a-910a-2351ff06c497";
            var superAdminUser = new IdentityUser()
            {
                Id = superAdminId,
                UserName = "superadmin",
                Email = "superadmin@arteconexao.com",
                NormalizedEmail = "superadmin@arteconexao.com".ToUpper(),
                NormalizedUserName = "superadmin".ToUpper(),
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin123*");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            var superAdminRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
