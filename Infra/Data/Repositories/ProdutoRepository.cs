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

        public async Task<List<ProdutoPedido>> AtualizarEstoqueDePedidosConfirmado(List<ProdutoPedido> produtos)
        {
            try
            {
                if (produtos.Count <= 0) throw new Exception("Não há produtos a serem atualizados no estoque.");
                foreach(var produto in produtos)
                {
                    if(!produto.Removed)
                    {
                        var produtoEncontrado = await _context.Set<Produto>()
                         .Where(p => p.Id == produto.ProdutoOriginalId)
                         .FirstAsync();

                        if (produtoEncontrado != null)
                        {
                            var balanco = produtoEncontrado.QuantidadeEstoque - produto.QuantidadePedido;
                            if (balanco < 0) throw new Exception("Não há unidades suficientes de " + produtoEncontrado.Nome + " disponíveis em estoque. Por favor, reduza a quantidade antes de concluir.");
                            produtoEncontrado.QuantidadeEstoque -= produto.QuantidadePedido;
                            _context.Set<Produto>().Update(produtoEncontrado);
                            //_context.SaveChanges();
                        }
                    }
                }
                _context.SaveChanges();
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