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
                FirstOrDefaultAsync();
        }

        public async Task<Pedido> BuscarPedidoPorId(int Id)
        {
            return await _context.Set<Pedido>().
                Where(p => p.Id == Id).
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


    }
}