using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Infra.Data.Context;

namespace LocalStore.Infra.Data.Repositories.Implementations
{
    public class RepositoryLayer : IRepositoryLayer
    {
        private readonly LocalStoreDbContext _context;
        public RepositoryLayer(LocalStoreDbContext context)
        {
            _context = context;
        }

        private readonly UserRepository? _user;
        public UserRepository User
        {
            get { return _user ?? new UserRepository(_context); }
        }

        private readonly EstabelecimentoRepository? _estabelecimento;
        public EstabelecimentoRepository Estabelecimento
        {
            get { return _estabelecimento ?? new EstabelecimentoRepository(_context); }
        }

        private readonly ClienteRepository? _cliente;
        public ClienteRepository Cliente
        {
            get { return _cliente ?? new ClienteRepository(_context); }
        }

        private readonly ProdutoRepository? _produto;
        public ProdutoRepository Produto
        {
            get { return _produto ?? new ProdutoRepository(_context); }
        }





        // Compartilhados entre todos repositórios

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task RollBackTransaction()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _context.Database.CommitTransactionAsync();
        }

    }
}
