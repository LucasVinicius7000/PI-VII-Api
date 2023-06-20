using LocalStore.Domain.DTO;

namespace LocalStore.Application.Requests
{
    public class CalculoDistanciaRequest
    {
        public Coordinates CoodernadasCliente { get; set; }
        public Coordinates CoodernadasEstabelecimento { get; set; }
    }
}
