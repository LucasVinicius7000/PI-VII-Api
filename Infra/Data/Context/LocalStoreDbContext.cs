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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Primary Keys

            builder.Entity<IdentityUser>()
                .HasKey(x => x.Id);

            builder.Entity<Estabelecimento>()
                .HasKey(e => e.Id);

            // Foreign Keys


            // Other Configurations

            builder.Entity<Estabelecimento>()
                .Ignore(e => e.Email);

        }

    }
}
