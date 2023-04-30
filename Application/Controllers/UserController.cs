using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Application.Controllers.Shared;

using LocalStore.Infra.Services.DistanceMatrix.Implementations;
using LocalStore.Domain.DTO;

namespace LocalStore.Application.Controllers
{
    [Route("user")]
    public class UserController : CustomBaseController<UserController>
    {
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IdentityUser>>> Login()
        {
            string Email = "lucascambraia@unipam.edu.br";
            string password = "Rootqwe123";
            
            try
            {
                var userToSignIn = await UserManager.FindByEmailAsync(Email);

                if(userToSignIn is null)
                {
                    throw new Exception("O email informado não corresponde a nenhum usuário cadastrado.");
                }

                var signInResult = await SignInManager.PasswordSignInAsync(userToSignIn, password, true, false);

                var response = new ApiResponse<IdentityUser>();
                if (signInResult is not null && signInResult.Succeeded)
                {
                    response = new ApiResponse<IdentityUser>().SucessResponse(userToSignIn, "Usuário logado com sucesso.");
                    return StatusCode(200, response);
                }
                else
                {
                    response = new ApiResponse<IdentityUser>().FailureResponse("Email ou senha incorretos.", null, null);
                    return StatusCode(500, response);
                }
                
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IdentityUser>().FailureResponse(ex.Message, "Exceção em: UserContoller:Login", ex);
                return StatusCode(500, response);
            }
        }

        [HttpGet("calculate")]
        [AllowAnonymous]
        public async Task<double> CalculateDistances()
        {
            var a = new Geolocation(Configuration);

            Coordinates cliente = new();
            cliente.Latitude = -18.405177;
            cliente.Longitude = -46.425473;
            Coordinates estabelecimento = new();
            estabelecimento.Latitude = -18.415871;
            estabelecimento.Longitude = -46.417255;

            //return await a.CalculateDistanceByCoordinates(cliente, estabelecimento);
            return await a.CalculateDistanceByCoordinates(cliente, estabelecimento);
  
        }

    }
}
