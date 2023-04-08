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

        [HttpGet(Name = "/")]
        [RequestSizeLimit(10000000000)]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        [HttpPost(Name = "GetWeatherForecast")]
        [RequestSizeLimit(10000000000)]
        public async Task<IEnumerable<WeatherForecast>> Get([FromBody] string base64)
        {

            byte[] meyarraydebites = Convert.FromBase64String(base64);

            MemoryStream stream = new(meyarraydebites);

            var retorno = await _blobStorageService.UploadFile("foto_de_teste", "png", stream);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Uri = retorno
            }).ToArray();


        }
    }
}