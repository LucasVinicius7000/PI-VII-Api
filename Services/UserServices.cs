using LocalStore.Services.Interfaces;

namespace LocalStore.Services
{
    public class UserServices
    {
        private readonly IServicesLayer _services;

        public UserServices
        (
            IServicesLayer services
        )
        {
            _services = services;
        }

    }
}
