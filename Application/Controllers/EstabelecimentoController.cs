﻿using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Application.Controllers.Shared;
using LocalStore.Application.Requests;
using LocalStore.Application.Responses;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;

namespace LocalStore.Application.Controllers
{
    [Route("estabelecimento")]
    public class EstabelecimentoController : CustomBaseController<EstabelecimentoController>
    {
        public EstabelecimentoController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<EstabelecimentoUsuarioResponse>> CadastrarEstabelecimento([FromBody] CriarEstabelecimentoUsuarioRequest request)
        {

            var estabelecimento = request.ToEstabelecimento();
            var userDTO = request.ToUserDTO();

            try
            {
                var novoEstabelecimento = await Services.Estabelecimento.CriarEstabelecimentoUsuario(estabelecimento, userDTO);  
                if (novoEstabelecimento is null)
                {
                    throw new Exception("Ocorreu um erro ao criar o usuário para o estabelecimento. ");
                }

                var estabelecimentoResponse = new EstabelecimentoUsuarioResponse(estabelecimento);
                estabelecimentoResponse.UserName = userDTO.UserName;
                estabelecimentoResponse.Email = userDTO.Email;

                var apiResponse = new ApiResponse<EstabelecimentoUsuarioResponse>().SucessResponse(estabelecimentoResponse, "Estabelecimento criado com sucesso. ");
                return StatusCode(200, apiResponse);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<EstabelecimentoUsuarioResponse>().FailureResponse("Não foi possível cadastrar o estabelecimento. " + ex.Message, "EstabelecimentoController:CadastrarEstabelecimento", ex);
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Estabelecimento>>> ListarEstabelecimentosProximos([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double raio)
        {

            try
            {
                Coordinates coordenadas = new Coordinates() { Latitude = latitude, Longitude = longitude };
                var listaEstabelecimentos = await Services.Estabelecimento.BuscarEstabelecimentosProximosPorRaio(coordenadas, raio);
                if (listaEstabelecimentos is null) throw new Exception("Falha ao listar estabelecimentos próximos");
                var apiResponse = new ApiResponse<List<Estabelecimento>>().SucessResponse(listaEstabelecimentos, "Estabelecimentos listados com sucesso.");
                return StatusCode(200, apiResponse);

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<List<Estabelecimento>>().FailureResponse("Ocorreu um erro listar os estabelecimentos próximos. " + ex.Message, "EstabelecimentoController:ListarEstabelecimentosProximos", ex);
                return StatusCode(500,apiResponse);
            }

        }

    }
}
