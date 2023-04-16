using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using LocalStore.Domain.Model;

namespace LocalStore.Infra.Data.Repositories
{
    public class EstabelecimentoRepository
    {
        public readonly LocalStoreDbContext _context;
        public EstabelecimentoRepository(LocalStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Estabelecimento> InsertEstabelecimento(Estabelecimento estabelecimento)
        {
            var result = await _context.Set<Estabelecimento>().AddAsync(estabelecimento);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

    }
}
