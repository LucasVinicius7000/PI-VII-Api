using LocalStore.Infra.Data.Context;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace LocalStore.Infra.Data.Repositories
{
    public class PedidoRepository
    {
        public readonly LocalStoreDbContext _context;

        public PedidoRepository(LocalStoreDbContext context)
        {
            _context = context;
        }

        
        public async Task<Pedido> BuscarPedidoAtual(int clienteId)
        {
            return await _context.Set<Pedido>().
                Where(p => p.ClienteId == clienteId && p.IsProdutoAtual).
                Include(p => p.ProdutosPedidos).
                Include(p => p.Estabelecimento).
                FirstOrDefaultAsync();
        }

        public async Task<Pedido> BuscarPedidoPorId(int Id)
        {
            return await _context.Set<Pedido>().
                Where(p => p.Id == Id).
                Include(p => p.ProdutosPedidos).
                FirstOrDefaultAsync();
        }

        public async Task<Pedido> CancelarPedido(int pedidoId)
        {
            var pedido = await _context.Set<Pedido>()
                .Where(p => p.Id == pedidoId)
                .FirstOrDefaultAsync();

            pedido.StatusPedido = Domain.Enum.StatusPedido.Cancelado;
            pedido.IsProdutoAtual = false;
            _context.Update(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> ConfirmarPedido(int pedidoId)
        {
            var pedido = await _context.Set<Pedido>()
                .Where(p => p.Id == pedidoId)
                .FirstOrDefaultAsync();

            pedido.StatusPedido = Domain.Enum.StatusPedido.Concluido;
            pedido.IsProdutoAtual = false;
            _context.Update(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> InsertPedido(Pedido pedidoMontado)
        {
            try
            {
                var pedidoCadastrado = _context.Set<Pedido>().Add(pedidoMontado);
                await _context.SaveChangesAsync();
                return pedidoCadastrado.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> AtualizarPedido(Pedido pedidoAtualizado)
        {
            var pedidoAtualizar = _context.Set<Pedido>()
                .Update(pedidoAtualizado).Entity;
            await _context.SaveChangesAsync();
            return pedidoAtualizar;
        }

        public async Task<List<Pedido>> BuscarTodosPedidosPorIdDoCliente(int clienteId)
        {
            return await _context.Set<Pedido>()
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.Estabelecimento)
                .Include(p => p.ProdutosPedidos)
                .ToListAsync();
        }

        public async Task<Pedido> RemoverProdutoDoPedido(int idPedido, int idProduto)
        {
            var produto = await _context.Set<Pedido>()
                .Where(p => p.Id == idPedido)
                .Include(p => p.ProdutosPedidos)
                .FirstAsync();

            produto.ProdutosPedidos.Where(prod => prod.Id == idProduto).First().Removed = true;

            var prod = _context.Set<Pedido>().Update(produto).Entity;
            await _context.SaveChangesAsync();
            return prod;

        }

        public async Task<Pedido> AlterarQtProdutoDoPedido(int idPedido, int idProduto, string operacao)
        {
            var produtoPedido = _context.Set<Pedido>()
                .Where(p => p.Id == idPedido)
                .Include(e => e.Estabelecimento)
                .Include(p => p.ProdutosPedidos)
                .First();

            
            if(operacao == "+")
            {
                produtoPedido.ProdutosPedidos.Where(prod => prod.Id == idProduto).First().QuantidadePedido++;
            }
            else if(operacao == "-")
            {
                produtoPedido.ProdutosPedidos.Where(prod => prod.Id == idProduto).First().QuantidadePedido--;
            }

            var entity = _context.Set<Pedido>().Update(produtoPedido).Entity;
            await _context.SaveChangesAsync();
            return entity;

        }


    }
}