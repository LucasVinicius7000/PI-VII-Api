using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Infra.Data.Repositories
{
    public class UserRepository
    {
        public readonly LocalStoreDbContext _context;
        public UserRepository(LocalStoreDbContext context)
        {
            _context = context;
        }

    }
}
