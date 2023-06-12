using Microsoft.AspNetCore.Identity;
using LocalStore.Infra.Data.Context;
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<EstabelecimentoUsuarioResponse>>> CadastrarEstabelecimento([FromBody] CriarEstabelecimentoUsuarioRequest request)
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

        [HttpGet("listar")]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<ActionResult<ApiResponse<List<Estabelecimento>>>> ListarEstabelecimentosProximos([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double raio)
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

        [HttpGet]
        [Authorize(Roles = "Admin, Estabelecimento, Cliente")]
        public async Task<ActionResult<ApiResponse<Estabelecimento>>> BuscarInfoEstabelecimento([FromQuery] string userId)
        {
            try
            {
                if (userId is null || userId == string.Empty) throw new Exception("Id do estabelecimento inválido.");
                var UserEstabelecimento = await Services.UserManager.FindByIdAsync(userId);
                if(UserEstabelecimento.Email is null || UserEstabelecimento.Email == string.Empty) throw new Exception("Id do estabelecimento inválido.");

                var estalecimento = await Services.Estabelecimento.BuscarEstabelecimentoPorEmail(UserEstabelecimento.Email);
                if (estalecimento is null) throw new Exception("Não foi encontrado nenhum estabelecimento para o id informado.");
                var apiResponse = new ApiResponse<Estabelecimento>().SucessResponse(estalecimento, "Estabelecimentos encontrado com sucesso.");
                return StatusCode(200, apiResponse);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Estabelecimento>().FailureResponse("Ocorreu um erro ao buscar o estabelecimento. " + ex.Message, "EstabelecimentoController:BuscarInfoEstabelecimento", ex);
                return StatusCode(500, apiResponse);
            }
        }

        [HttpGet("info")]
        [Authorize(Roles = "Admin, Cliente")]
        public async Task<ActionResult<ApiResponse<Estabelecimento>>> BuscarInfoEstabelecimentoPorId([FromQuery] int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Id do estabelecimento inválido.");
                var estalecimento = await Services.Estabelecimento.BuscarEstabelecimentoPorId(id);
                if (estalecimento is null) throw new Exception("Não foi encontrado nenhum estabelecimento para o id informado.");
                var apiResponse = new ApiResponse<Estabelecimento>().SucessResponse(estalecimento, "Estabelecimentos encontrado com sucesso.");
                return StatusCode(200, apiResponse);

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Estabelecimento>().FailureResponse("Ocorreu um erro ao buscar o estabelecimento. " + ex.Message, "EstabelecimentoController:BuscarInfoEstabelecimento", ex);
                return StatusCode(500, apiResponse);
            }
        }

        [HttpPost("submitForm")]
        [Authorize(Roles = "Estabelecimento")]
        public async Task<ActionResult<ApiResponse<Estabelecimento>>> SubmeterFormularioDeAplicacao(FormularioSubmetidoRequest formulario)
        {
            
            try
            {
                if (formulario is null) throw new Exception("Formulário submetido inválido, tente novamente.");
                var content = new MemoryStream(Convert.FromBase64String(formulario.ConteudoArquivo));
                var arquivoUrl = await BlobStorage.UploadFile(formulario.NomeArquivo, formulario.ExtensaoArquivo, content);
                if (arquivoUrl == null || arquivoUrl == String.Empty)
                {
                    throw new Exception("Ocorreu uma falha ao cadastrar arquivo, tente novamente.");
                }
                var estabelecimento = await Services.Estabelecimento.BuscarEstabelecimentoPorId(formulario.EstabelecimentoId);
                if (estabelecimento is null) throw new Exception("Não foi possível encontrar o estabelecimento.");
                estabelecimento.Latitude = formulario.Coordenadas.Latitude;
                estabelecimento.Longitude = formulario.Coordenadas.Longitude;
                estabelecimento.CPFProprietario = formulario.CPFProprietario;
                estabelecimento.NomeProprietario = formulario.NomeProprietario;
                estabelecimento.TaxaKmRodado = formulario.ValorPorKmRodado;
                estabelecimento.TaxaMinimaEntrega = formulario.TaxaMinima;
                estabelecimento.UrlAlvaraFuncionamento = arquivoUrl;
                estabelecimento.MetodoCompra = formulario.MetodoCompra;
                estabelecimento.FormasPagamentoAceitas = formulario.FormasPagamento;
                estabelecimento.Endereco = formulario.Endereco;
                estabelecimento.Aprovado = Domain.Enum.StatusAprovacao.PendenteAprovacao;

                var estabelecientoAtualizado = await Services.Estabelecimento.SubmeterFormularioDeAplicacao(estabelecimento);


                var apiResponse = new ApiResponse<Estabelecimento>().SucessResponse(estabelecientoAtualizado, "Formulário de aplicação submetido com sucesso.");
                return StatusCode(200, apiResponse);

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse<Estabelecimento>().FailureResponse("Ocorreu um erro ao submeter o formulário, tente novamente mais tarde. " + ex.Message, "EstabelecimentoController:SubmeterFormularioDeAplicacao", ex);
                return StatusCode(500, apiResponse);
            }

        }
    }
}
