using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Application.Controllers.Shared;
using LocalStore.Application.Requests;
using LocalStore.Application.Responses;
using Microsoft.AspNetCore.Authorization;

namespace LocalStore.Application.Controllers
{
    [Route("produto")]
    public class ProdutoController : CustomBaseController<ProdutoController>
    {

        public ProdutoController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpPost("cadastrar")]
        [Authorize(Roles = "Estabelecimento")]
        public async Task<ActionResult<ApiResponse<ProdutoResponse>>> CadastrarProduto([FromBody] CreateProdutoRequest produtoRequest)
        {
            try
            {
                var produtoCadastrar = produtoRequest.ToProduto();
                if (produtoRequest.ConteudoArquivoImagem != String.Empty && produtoRequest.NomeArquivoImagem != String.Empty && produtoRequest.ExtensaoArquivoImagem != String.Empty)
                {
                    var contentImage = new MemoryStream(Convert.FromBase64String(produtoRequest.ConteudoArquivoImagem));
                    produtoCadastrar.UrlImagem = await BlobStorage.UploadImageFile(produtoRequest.NomeArquivoImagem, produtoRequest.ExtensaoArquivoImagem, contentImage);
                    if(produtoCadastrar.UrlImagem == null || produtoCadastrar.UrlImagem == String.Empty)
                    {
                        throw new Exception("Ocorreu uma falha ao cadastrar imagem do produto, tente novamente.");
                    }
                }

                var produtoCadastrado = await Services.Produto.CadastrarProduto(produtoCadastrar);
                if (produtoCadastrado != null)
                {
                    var produtoResponse = new ProdutoResponse(produtoCadastrado);
                    var apiResponse = new ApiResponse<ProdutoResponse>().SucessResponse(produtoResponse, "Produto " + produtoCadastrado.Nome + " cadastrado com sucesso.");
                    return StatusCode(200, apiResponse);
                }
                else throw new Exception("Falha ao cadastrar o produto, tente novamente mais tarde.");

            }
            catch (Exception ex)
            {
                var apiReponse = new ApiResponse<ProdutoResponse>().FailureResponse(ex.Message, "ProdutoController:CadastrarProduto", ex);
                return StatusCode(500, apiReponse);
            }
        }

        [HttpGet("")]
        [Authorize(Roles = "Estabelecimento, Cliente")]
        public async Task<ActionResult<ApiResponse<ProdutoResponse>>> BuscarProduto([FromQuery] int id)
        {
            try
            {
                var produtoEncontrado = await Services.Produto.BuscarProdutoPorId(id);;
                if (produtoEncontrado != null)
                {
                    var produtoResponse = new ProdutoResponse(produtoEncontrado);
                    var apiResponse = new ApiResponse<ProdutoResponse>().SucessResponse(produtoResponse, "Produto " + produtoEncontrado.Nome + " encontrado com sucesso.");
                    return StatusCode(200, apiResponse);
                }
                else throw new Exception("Falha ao buscar o produto.");

            }
            catch (Exception ex)
            {
                var apiReponse = new ApiResponse<ProdutoResponse>().FailureResponse(ex.Message, "ProdutoController:BuscarProduto", ex);
                return StatusCode(500, apiReponse);
            }
        }

    }
}
