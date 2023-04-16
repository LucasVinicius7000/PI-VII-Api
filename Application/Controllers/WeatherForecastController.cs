using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Infra.BlobStorage.Interfaces;
using LocalStore.Controllers.Shared;

namespace LocalStore.Controllers
{
    public class WeatherForecastController : CustomBaseController<WeatherForecastController>
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet("Get")]
        [RequestSizeLimit(1000000000)]
        public async Task<ActionResult<ApiResponse<WeatherForecast[]>>> Get([FromQuery] string username, [FromQuery] string email)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = username,
                    Email = email
                };

                var result = await UserManager.CreateAsync(user, "123Pa$$word.");

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            var response = new ApiResponse<WeatherForecast[]>()
                .SucessResponse(data,"");

            return response;
        }
    }
}