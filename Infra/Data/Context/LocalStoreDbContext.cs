using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LocalStore.Domain.Model;

namespace LocalStore.Infra.Data.Context
{
    public class LocalStoreDbContext : IdentityDbContext
    {
        public LocalStoreDbContext(DbContextOptions<LocalStoreDbContext> options) : base(options) { }

        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Primary Keys

            builder.Entity<IdentityUser>()
                .HasKey(x => x.Id);

            builder.Entity<Estabelecimento>()
                .HasKey(e => e.Id);

            builder.Entity<Cliente>()
                .HasKey(c => c.Id);

            // Foreign Keys


            // Other Configurations


        }

    }
}
