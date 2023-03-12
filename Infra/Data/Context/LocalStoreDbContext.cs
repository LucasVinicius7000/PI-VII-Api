using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalStore.Infra.Data.Context
{
    public class LocalStoreDbContext : IdentityDbContext
    {
        public LocalStoreDbContext(DbContextOptions<LocalStoreDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>()
                .HasKey(x => x.Id);

        }

    }
}
