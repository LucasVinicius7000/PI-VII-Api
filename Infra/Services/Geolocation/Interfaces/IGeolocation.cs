using LocalStore.Domain.DTO;
using System.Text.Json.Nodes;

namespace LocalStore.Infra.Services.DistanceMatrix.Interfaces
{
    public interface IGeolocation
    {
        Task<double> CalculateDistanceByCoordinates(Coordinates origin, Coordinates destination);
    }
}
