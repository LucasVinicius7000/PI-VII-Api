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

        private readonly UserRepository? _userRepository;
        public UserRepository UserRepository
        {
            get { return _userRepository ?? new UserRepository(_context); }
        }

    }
}
