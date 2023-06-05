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
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

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

            builder.Entity<Produto>()
                .HasKey(p => p.Id);

            builder.Entity<Pedido>()
                .HasKey(p => p.Id);

            builder.Entity<ProdutoPedido>()
                .HasKey(p => p.Id);


            // Foreign Keys

            builder.Entity<Estabelecimento>()
                .HasMany(e => e.Produtos)
                .WithOne(p => p.Estabelecimento)
                .HasForeignKey(p => p.Id);

            builder.Entity<Produto>()
                .HasOne(p => p.Estabelecimento)
                .WithMany(e => e.Produtos)
                .HasForeignKey(p => p.EstabelecimentoId);

            builder.Entity<Pedido>()
                .HasOne(p => p.Estabelecimento)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.EstabelecimentoId);

            builder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId);

            builder.Entity<ProdutoPedido>()
                .HasOne(p => p.Pedido)
                .WithMany(p => p.ProdutosPedidos)
                .HasForeignKey(p => p.PedidoId);

            // Other Configurations


        }

    }
}
