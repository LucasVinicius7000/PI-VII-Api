using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Application.Controllers.Shared;
using Microsoft.AspNetCore.Identity;
using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Authorization;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Application.Requests;
using LocalStore.Application.Responses;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;

namespace LocalStore.Application.Controllers
{
    [Route("pedido")]
    public class PedidoController : CustomBaseController<PedidoController>
    {
        public PedidoController(IServiceProvider serviceProvider) : base(serviceProvider) { }


        [HttpGet("atual")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<Pedido>> BuscaPedidoAtual([FromQuery] int ClienteId)
        {
            try
            {
                var pedidoAtual = new Pedido();
                var clienteExiste = await Services.Cliente.BuscarClientePeloId(ClienteId);
                if (clienteExiste == null) throw new Exception("O cliente não existe na base de dados");
                else
                {
                     pedidoAtual = await Services.Pedido.BuscarPedidoAtual(ClienteId);
                }

                var apiResponse = new ApiResponse<Pedido>().SucessResponse(pedidoAtual, "Pedido #" + pedidoAtual.Id + " encontrado com sucesso.");
                return StatusCode(200, apiResponse);

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Pedido>().FailureResponse("Pedido atual não foi encontrado.", ex.Message, ex);
                return StatusCode(500,apiResponse);
            }
        }

        [HttpPatch("cancelar")]
        [Authorize(Roles = "Cliente, Estabelecimento")]
        public async Task<ActionResult> CancelarPedido([FromQuery] int pedidoId)
        {
            try
            {
                if (pedidoId <= 0) throw new Exception("O id informado não corresponde um pedido cadastrado.");
                var pedidoASerCancelado = await Services.Pedido.CancelarPedido(pedidoId);
                var apiResponse = new ApiResponse<Pedido>().SucessResponse(pedidoASerCancelado, "Pedido #" + pedidoASerCancelado.Id + " cancelado com sucesso.");
                return StatusCode(200, apiResponse);

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Pedido>().FailureResponse("Pedido não pôde ser cancelado.", ex.Message, ex);
                return StatusCode(500, apiResponse);
            }
        }

        [HttpPost("criarOUeditar")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult> CriarPedidoOuEditar([FromBody] AddProdutoRequest produtoAdicionar)
        {
            try
            {
                var pedidoCriadoAtualizado = new Pedido();
                if (produtoAdicionar == null) throw new Exception("Erro ao adicionar produto ao pedido atual.");
                var produtoOriginal = await Services.Produto.BuscarProdutoPorId(produtoAdicionar.ProdutoIdOriginal);
                if (produtoOriginal == null) throw new Exception("O produto que deseja adicionar não existe na base de dados.");
                var pedidoJaExiste = await Services.Pedido.BuscarPedidoAtual(produtoAdicionar.ClienteId);
                if (pedidoJaExiste != null)
                {
                    pedidoCriadoAtualizado = await Services.Pedido.AtualizarProdutosPedidos(pedidoJaExiste, produtoOriginal);
                    if (pedidoCriadoAtualizado == null) throw new Exception("Ocorreu um erro ao criar pedido e adicionar produto.");
                }
                else
                {
                    pedidoCriadoAtualizado = await Services.Pedido.CriarPedidoEAdicionarProduto(produtoAdicionar.ClienteId, produtoOriginal);
                    if (pedidoCriadoAtualizado == null) throw new Exception("Ocorreu um erro ao criar pedido e adicionar produto.");
                }
                var apiResponse = new ApiResponse<Pedido>().SucessResponse(pedidoCriadoAtualizado, "Pedido #" + pedidoCriadoAtualizado.Id + " atualizado com sucesso.");
                return StatusCode(200, apiResponse);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Pedido>().FailureResponse("Pedido não pôde ser atualizado com novo produto.", ex.Message, ex);
                return StatusCode(500, apiResponse);
            }
        }

        [HttpGet("lista")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<List<Pedido>>> ListarPedidosPorId([FromQuery] int clienteId)
        {
            try
            {
                if (clienteId <= 0) throw new Exception("O id do cliente não é válido.");
                var lista = await Services.Pedido.ListarPedidosPorClienteId(clienteId);
                var apiresponse = new ApiResponse<List<Pedido>>().SucessResponse(lista, "Pedidos listados com sucesso.");
                return StatusCode(200, apiresponse);
            }
            catch (Exception ex)
            {
                var apiresponse = new ApiResponse<List<Pedido>>().FailureResponse("Falha ao lista pedidos do cliente.", ex.Message, ex);
                return StatusCode(500, apiresponse);
            }
        }

    }
}
