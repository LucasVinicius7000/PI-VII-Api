using LocalStore.Infra.Data.Context;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace LocalStore.Infra.Data.Repositories
{
    public class ClienteRepository
    {
        public readonly LocalStoreDbContext _context;

        public ClienteRepository(LocalStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> InsertCliente(Cliente cliente) 
        {
            var result = await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Cliente> BuscarClientePeloUserId(string userId)
        {
            return await _context.Set<Cliente>().Where(c => c.UserId == userId).FirstOrDefaultAsync();
        }



    }
}
