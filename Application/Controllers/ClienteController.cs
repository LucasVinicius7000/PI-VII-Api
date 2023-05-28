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


    }
}
