using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocalStore.Application.Controllers.Shared
{
  

    [Route("create")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        [HttpGet("")]
        public async Task<ActionResult<JToken>> Get([FromQuery] string url)
        {

            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Remove("Sec-Fetch-Site");

                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(requestMessage);


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
