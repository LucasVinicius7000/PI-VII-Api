using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Infra.BlobStorage.Implementations;
using LocalStore.Infra.BlobStorage.Interfaces;


namespace LocalStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly LocalStoreDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private IBlobStorageService _blobStorageService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, LocalStoreDbContext context, UserManager<IdentityUser> userManager, IBlobStorageService blogStorageService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _blobStorageService = blogStorageService;
        }

        [HttpGet("Get")]
        [RequestSizeLimit(1000000000)]
        public async Task<IEnumerable<WeatherForecast>> Get([FromQuery] string username, [FromQuery] string email)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = username,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, "123Pa$$word.");

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}