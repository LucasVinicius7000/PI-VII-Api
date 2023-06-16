using LocalStore.Infra.Services.DistanceMatrix.Interfaces;
using Microsoft.Net.Http;
using LocalStore.Domain.DTO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace LocalStore.Infra.Services.DistanceMatrix.Implementations
{
    public class Geolocation : IGeolocation 
    {

        private readonly IConfiguration _configuration;
        private string ApiKey { get; set; } = string.Empty;
        private string Url { get; set; } = string.Empty;

        public Geolocation(IConfiguration configuration)
        {
            _configuration = configuration;
            this.ApiKey = _configuration.GetValue<string>("GoogleCloudSecrets:DistanceMatrixApiKey");
        }

        public async Task<double> CalculateDistanceByCoordinates(Coordinates origin, Coordinates destination)
        {
            try
            {
                this.Url = _configuration.GetValue<string>("GoogleMapsApisBaseUrls:DistanceMatrix");
                JsonDocument result;
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{Url}origins={origin.Latitude},{origin.Longitude}&destinations={destination.Latitude},{destination.Longitude}&key={ApiKey}");
                    if (response.IsSuccessStatusCode && response.Content != null)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result = JsonDocument.Parse(content);
                        var status = result.RootElement.GetProperty("rows")[0].GetProperty("elements")[0].GetProperty("status").ToString();
                        if(status == "OK")
                        {
                            string distance = result.RootElement.GetProperty("rows")[0].GetProperty("elements")[0].GetProperty("distance").GetProperty("text").ToString();
                            if(distance.Contains("km"))
                            {
                                distance = Regex.Replace(distance, " km", "");
                            }
                            else if(distance.Contains("m"))
                            {
                                distance = Regex.Replace(distance, " m", "");
                            }
                            return Double.Parse(distance, CultureInfo.InvariantCulture);
                        }
                        else if (status == "OVER_QUERY_LIMIT" || status == "OVER_QUERY_LIMIT")
                        {
                            throw new Exception("A chave da api é inválida ou o limite de solicitações diárias foi atingido.");
                        }
                        else throw new Exception("A solicitação de distância entre as coordenadas falhou por um erro desconhecido. Tente novamente mais tarde.");
                    }
                    else throw new Exception($"A solicitação de distância entre as coordenadas falhou com status code: {response.StatusCode}. {response.ReasonPhrase}.");
                    
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Coordinates> GetCordinatesByGeocodifcation(string address)
        {
            try
            {
                this.Url = _configuration.GetValue<string>("GoogleMapsApisBaseUrls:Geocoding");

                JsonDocument result;
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{Url}address={address}&key={ApiKey}");
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonDocument.Parse(content);
                };


            }
            catch (Exception)
            {

                throw;
            }
            return new Coordinates();
        }


    }
}
