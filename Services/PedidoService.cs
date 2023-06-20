using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Services.Interfaces;
using LocalStore.Application.Controllers;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Services
{
    public class PedidoService
    {
        private readonly IServicesLayer _services;
        private readonly IRepositoryLayer _repositories;

        public PedidoService(IServicesLayer services, IRepositoryLayer repositories)
        {
            _services = services;
            _repositories = repositories;
        }

        public async Task<Pedido> BuscarPedidoAtual(int clienteId)
        {
            try
            {
                var pedidoAtual = new Pedido();
                if (clienteId <= 0) throw new Exception("O id informado não corresponde a nenhum cliente cadastrado.");
                else
                {
                    pedidoAtual = await _repositories.Pedido.BuscarPedidoAtual(clienteId);
                    if (pedidoAtual == null) return null;
                    foreach(var produto in pedidoAtual.ProdutosPedidos)
                    {
                        var original = await _repositories.Produto.BuscarProdutoPorId(produto.ProdutoOriginalId);
                        produto.QuantidadeMax = original.QuantidadeEstoque;
                    }
                    return pedidoAtual;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> CancelarPedido(int pedidoId)
        {
            try
            {
                if (pedidoId <= 0) throw new Exception("Não foi possível cancelar produto, id não é válido.");
                await _repositories.BeginTransaction();
                var pedidoCancelado = await _repositories.Pedido.CancelarPedido(pedidoId);
                if (pedidoCancelado == null) throw new Exception("Pedido não encontrado, impossível cancelar.");
                //var produtosRestaurados = await _repositories.Produto.RestaurarEstoqueDePedidosCancelados(pedidoCancelado.ProdutosPedidos);
                //if (produtosRestaurados == null) throw new Exception("Falha ao devolver produtos do pedido cancelado ao estoque.");
                var pedido = await _repositories.Pedido.BuscarPedidoPorId(pedidoId);
                if (pedido == null) throw new Exception("Pedido cancelado não encontrado.");
                await _repositories.CommitTransaction();
                return pedido;
            }
            catch (Exception ex)
            {
                await _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }
       
        public async Task<Pedido> CriarPedidoEAdicionarProduto(int clienteId,Produto produtoOriginal, double quantidade)
        {
            try
            {
                var listProdutos = new List<ProdutoPedido>();
                var produtoPedido = new ProdutoPedido()
                {
                    Categoria = produtoOriginal.Categoria,
                    Lote = produtoOriginal.Lote,
                    Marca = produtoOriginal.Marca,
                    Nome = produtoOriginal.Nome,
                    Observacao = produtoOriginal.Observacao,
                    Peso = produtoOriginal.Peso,
                    ProdutoOriginalId = produtoOriginal.Id,
                    ValorComDesconto = produtoOriginal.ValorComDesconto,
                    ValorUnitario = produtoOriginal.ValorUnitario,
                    QuantidadePedido = quantidade,
                    UnidadeMedida = produtoOriginal.UnidadeMedida,
                    UrlImagem = produtoOriginal.UrlImagem,
                    VencimentoEm = produtoOriginal.VencimentoEm,
                    VendidoPor = produtoOriginal.VendidoPor,
                };
                listProdutos.Add(produtoPedido);
                var pedidoFeito = new Pedido()
                {
                    ClienteId = clienteId,
                    EstabelecimentoId = produtoOriginal.EstabelecimentoId,
                    IsProdutoAtual = true,
                    StatusPedido = Domain.Enum.StatusPedido.EmAndamento,
                    ProdutosPedidos = listProdutos,
                };
                await _repositories.BeginTransaction();
                var pedidoCadastrado = await _repositories.Pedido.InsertPedido(pedidoFeito);
                if (pedidoCadastrado == null) throw new Exception("Falha ao cadastrar pedido.");
                await _repositories.CommitTransaction();
                return pedidoCadastrado;
            }
            catch (Exception ex)
            {
                await _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> AtualizarProdutosPedidos(Pedido pedido, Produto produtoOriginal, double quantidade)
        {
            try
            {
                await _repositories.BeginTransaction();
                var produtoPedido = new ProdutoPedido()
                {
                    Categoria = produtoOriginal.Categoria,
                    Lote = produtoOriginal.Lote,
                    Marca = produtoOriginal.Marca,
                    Nome = produtoOriginal.Nome,
                    Observacao = produtoOriginal.Observacao,
                    Peso = produtoOriginal.Peso,
                    ProdutoOriginalId = produtoOriginal.Id,
                    ValorComDesconto = produtoOriginal.ValorComDesconto,
                    ValorUnitario = produtoOriginal.ValorUnitario,
                    QuantidadePedido = quantidade,
                    UnidadeMedida = produtoOriginal.UnidadeMedida,
                    UrlImagem = produtoOriginal.UrlImagem,
                    VencimentoEm = produtoOriginal.VencimentoEm,
                    VendidoPor = produtoOriginal.VendidoPor,
                    PedidoId = pedido.Id,
                };
                if (pedido == null || produtoOriginal == null) throw new Exception("Falha ao ler informações do pedido.");
                pedido.ProdutosPedidos.Add(produtoPedido);
                var result = await _repositories.Pedido.AtualizarPedido(pedido);
                if (result == null) throw new Exception("Falha ao atualizar pedido com novo produto.");
                await _repositories.CommitTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Pedido>> ListarPedidosPorClienteId(int clienteId)
        {
            try
            {
                if (clienteId <= 0) throw new Exception("O id do cliente é inválido, não foi possível listar pedidos.");
                var listaPedidos = await _repositories.Pedido.BuscarTodosPedidosPorIdDoCliente(clienteId);
                if (listaPedidos == null) throw new Exception("Ocorreu uma falha ao listar todos pedidos do cliente atual.");
                return listaPedidos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> RemoverPedido(int idPedido, int idProduto)
        {
            try
            {
                if (idPedido <= 0) throw new Exception("O id do pedido é inválido, não foi possível remover produto.");
                var removido = await _repositories.Pedido.RemoverProdutoDoPedido(idPedido, idProduto);
                if (removido == null) throw new Exception("Ocorreu um erro remover pedido");
                return removido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> AlterarQuantidadePedido(int idPedido, int idProduto, string operacao)
        {
            try
            {
                if (idPedido <= 0) throw new Exception("O id do pedido é inválido.");
                var alterado = await _repositories.Pedido.AlterarQtProdutoDoPedido(idPedido, idProduto, operacao);
                if (alterado == null) throw new Exception("Ocorreu um erro ao alterar quantidade do produto.");
                return alterado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pedido> ConcluirPedidoAtualizarEstoque(int idPedido)
        {
            try
            {
                await _repositories.BeginTransaction();
                var pedido = await _repositories.Pedido.BuscarPedidoPorId(idPedido);
                if (pedido == null) throw new Exception("Pedido não encontrado, falha ao confirmar.");
                var estoqueAtualizado = await _repositories.Produto.AtualizarEstoqueDePedidosConfirmado(pedido.ProdutosPedidos);
                var pedidoConfirmado = await _repositories.Pedido.ConfirmarPedido(idPedido);
                if (pedidoConfirmado == null) throw new Exception("Falha ao confirmar pedido.");
                await _repositories.CommitTransaction();
                return pedidoConfirmado;
            }
            catch (Exception ex)
            {
                await _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }

    }
}