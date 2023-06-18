using LocalStore.Infra.Data.Context;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace LocalStore.Infra.Data.Repositories
{
    public class ProdutoRepository
    {
        public readonly LocalStoreDbContext _context;

        public ProdutoRepository(LocalStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> InsertProduto(Produto produto)
        {
            var result = await _context.Set<Produto>().AddAsync(produto);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Produto>> BuscarProdutosPorEstabelecimentoId(int estabelecimentoId)
        {
            return await _context.Set<Produto>()
                .Where(x => x.EstabelecimentoId == estabelecimentoId)
                .ToListAsync();
        }

        public async Task<Produto> BuscarProdutoPorId(int Id)
        {
            return await _context.Set<Produto>()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProdutoPedido>> RestaurarEstoqueDePedidosCancelados(List<ProdutoPedido> produtos)
        {
            try
            {
                if (produtos.Count <= 0) throw new Exception("Não há produtos a serem restaurados no estoque.");
                foreach(var produto in produtos)
                {
                    var produtoEncontrado = await _context.Set<Produto>()
                         .Where(p => p.Id == produto.ProdutoOriginalId)
                         .FirstAsync();

                    if(produtoEncontrado != null)
                    {
                        produtoEncontrado.QuantidadeEstoque += produto.QuantidadePedido;
                        _context.Set<Produto>().Update(produtoEncontrado);
                        _context.SaveChanges();
                    }
                }
                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        //public async Task<List<ProdutoPedido>> AlterarEstoqueDePedidosConfirmados(List<ProdutoPedido> produtos)
        //{

        //}

    }
}