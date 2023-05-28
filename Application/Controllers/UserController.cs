using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Application.Controllers.Shared;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;
using LocalStore.Domain.DTO;
using LocalStore.Application.Requests;
using LocalStore.Application.Responses;

namespace LocalStore.Application.Controllers
{
    [Route("user")]
    public class UserController : CustomBaseController<UserController>
    {
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var userToSignIn = new IdentityUser();
                if (loginRequest.Email != string.Empty)
                {
                    userToSignIn = await UserManager.FindByEmailAsync(loginRequest.Email);
                }
                else if (loginRequest.UserName != string.Empty)
                {
                    userToSignIn = await UserManager.FindByNameAsync(loginRequest.UserName);
                }
                else throw new Exception("O email ou usuário informado é inválido.");

                if(userToSignIn is null)
                {
                    throw new Exception("O email informado não corresponde a nenhum usuário cadastrado.");
                }

                var signInResult = await SignInManager.PasswordSignInAsync(userToSignIn, loginRequest.Password, true, false);

                string token = String.Empty;
                List<string> role;
                if (signInResult.Succeeded)
                {
                    role = (List<string>)await Services.UserManager.GetRolesAsync(userToSignIn);
                    if (role[0] == "Estabelecimento")
                    {
                        var estabelecimento = await Services.Estabelecimento.BuscarEstabelecimentoPorEmail(userToSignIn.Email);
                        if (estabelecimento == null) throw new Exception("Falha ao gerar token do usário. Id do estabelecimento não encontrado.");
                        token = Services.TokenService.GenerateToken(userToSignIn, role, estabelecimento.Id.ToString());
                    }
                    else if (role[0] == "Admin")
                    {
                        token = Services.TokenService.GenerateToken(userToSignIn, role, userToSignIn.Id);
                    }
                    else if (role[0] == "Cliente")
                    {
                        var cliente = await Services.Cliente.BuscarClientePeloUserId(userToSignIn.Id);
                        if(cliente == null) throw new Exception("Falha ao gerar token do usário. Id do cliente não encontrado.");
                        token = Services.TokenService.GenerateToken(userToSignIn, role, cliente.Id.ToString());
                    }
                    
                }
                else throw new Exception("Falha ao realizar login, email ou senha incorretos.");

                var response = new ApiResponse<LoginResponse>();

                if (signInResult is not null && token != String.Empty && role != null)
                {
                    var loginResponse = new LoginResponse()
                    {
                        Role = role[0],
                        Token = token,
                        Email = userToSignIn.Email,
                        Id = userToSignIn.Id,
                        UserName = userToSignIn.UserName,
                        IsApproved = true
                    };
                    response = new ApiResponse<LoginResponse>().SucessResponse(loginResponse, "Usuário logado com sucesso.");
                    return StatusCode(200, response);
                }
                else
                {
                    response = new ApiResponse<LoginResponse>().FailureResponse("Email ou senha incorretos.", null, null);
                    return StatusCode(500, response);
                }
                
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IdentityUser>().FailureResponse(ex.Message, "Exceção em: UserContoller:Login", ex);
                return StatusCode(500, response);
            }
        }

    }
}
