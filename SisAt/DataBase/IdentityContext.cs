using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SisAt.Models;
using System.Reflection;
using SisAt.DataBase.Mapping;

namespace SisAt.DataBase
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<Usuario, IdentityRole<long>, long, IdentityUserClaim<long>, IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IdentityUserMapping());
            modelBuilder.ApplyConfiguration(new IdentityRoleClaimMapping());
            modelBuilder.ApplyConfiguration(new IdentityRoleMapping());
            modelBuilder.ApplyConfiguration(new IdentityUserClaimMapping());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginMapping());
            modelBuilder.ApplyConfiguration(new IdentityUserRoleMapping());
            modelBuilder.ApplyConfiguration(new IdentityUserTokenMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
