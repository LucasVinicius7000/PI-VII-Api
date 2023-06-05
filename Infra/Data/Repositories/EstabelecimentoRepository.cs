using LocalStore.Infra.Data.Context;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Estabelecimento>> ListaEstabelecimentosPorRaioECoordenadas(Coordinates coordinates, double raio)
        {
            double distanciaMaximaKm = raio + 2;

            var result = await _context.Set<Estabelecimento>()
                .FromSqlRaw(@"SELECT * FROM estabelecimentos as E
                              WHERE(6371 * acos(cos(radians(" + coordinates.Latitude + @"))
                              *cos(radians(E.Latitude))
                              * cos(radians(E.Longitude) - radians(" + coordinates.Longitude + @"))
                              + sin(radians(" + coordinates.Latitude + @"))
                              * sin(radians(E.Latitude)))) <= " + distanciaMaximaKm + @";"
                ).ToListAsync();

            return result;

        }

        public async Task<Estabelecimento> BuscarEstabelecimentoPeloId(int Id)
        {
            return await _context.Set<Estabelecimento>()
                .Where(e => e.Id == Id)
                .FirstOrDefaultAsync();

        }

        public async Task<Estabelecimento> BuscarEstabelecimentoPeloEmail(string Email)
        {
            return await _context.Set<Estabelecimento>()
                .Where(e => e.Email == Email)
                .Include(e => e.Produtos)
                .FirstOrDefaultAsync();

        }

        public async Task<Estabelecimento> AtualizarEstabelecimento(Estabelecimento estabelecimento)
        {
            var estabelecimentoAtualizado = _context.Estabelecimentos.Update(estabelecimento);
            await _context.SaveChangesAsync();
            return estabelecimento;
        }

    }
}
