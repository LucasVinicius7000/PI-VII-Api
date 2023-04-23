using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocalStore.Application.Controllers.Shared
{
  

    [Route("proxy")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
  

        [HttpGet("")]
        public async Task<ActionResult<JToken>> Get([FromQuery] string url)
        {
            try
            {
                using(var client = new HttpClient())
                {
                    // Realiza a requisição para o endpoint desejado utilizando o HttpClient
                    var response = await client.GetAsync(url);

                    // Lê o conteúdo da resposta como um objeto JToken
                    var content = await response.Content.ReadAsStringAsync();

                    JToken json = JObject.Parse(content);
                    
                    return new JsonResult(json);
                }
                
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }
        }
    }
}
