using LocalStore.Application.Controllers.Shared;
using Microsoft.AspNetCore.Identity;
using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Application.Requests;
using LocalStore.Application.Responses;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;

namespace LocalStore.Application.Controllers
{
    [Route("cliente")]
    public class ClienteController : CustomBaseController<ClienteController>
    {
        public ClienteController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpPost("cadastro")]
        [AllowAnonymous]
        public async Task<ActionResult<ClienteResponse>> CriarUsuarioCliente([FromBody] CreateClienteRequest clienteRequest)
        {
            try
            {
                if (clienteRequest == null) throw new Exception("Os dados informados são inválidos");
                var cliente = clienteRequest.ToCliente();
                var user = clienteRequest.ToUserDTO();
                var clienteCadastrado = await Services.Cliente.CriarUsuarioCliente(cliente, user);
                if (clienteCadastrado is null)
                {
                    throw new Exception("Ocorreu um erro ao criar o usuário para o cliente. ");
                }

                var clienteResponse = new ClienteResponse(clienteCadastrado, user);
                var apiResponse = new ApiResponse<ClienteResponse>().SucessResponse(clienteResponse, "Usuário criado com sucesso. ");
                return StatusCode(200, apiResponse);


            }
            catch (Exception ex)
            {
                var response = new ApiResponse<ClienteResponse>().FailureResponse("Não foi possível cadastrar o usuário. " + ex.Message, "ClienteController:CriarUsuarioCliente", ex);
                return StatusCode(500, response);
            }
        }

        [HttpPost("distanciaClienteEstabelecimento")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult> CalculaDistanciaEstabelecimentoCliente([FromBody] CalculoDistanciaRequest coordenadas)
        {
            try
            {
                if (coordenadas.CoodernadasEstabelecimento == null) throw new Exception("Coordenadas do estabelecimento inválidas.");
                if (coordenadas.CoodernadasCliente == null) throw new Exception("Coordenadas do cliente inválidas.");
                if (coordenadas == null) throw new Exception("Coordenadas inválidas.");
                var geo = new Geolocation(Configuration);

                var response = new CalculoDistanciaResponse()
                {
                    DistanciaEmKm = await geo.CalculateDistanceByCoordinates(coordenadas.CoodernadasCliente, coordenadas.CoodernadasEstabelecimento)
                };
                if (response.DistanciaEmKm == null) throw new Exception("Erro ao calcular distancia entre estabelecimento e cliente.");

                var apiResponse = new ApiResponse<CalculoDistanciaResponse>().SucessResponse(response, "Distancia calculada com sucesso");
                return StatusCode(200, apiResponse);


            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<CalculoDistanciaResponse>().FailureResponse("Falha ao calcular distancia", ex.Message, ex);
                return StatusCode(500, apiResponse);
            }
        }
    }
}
