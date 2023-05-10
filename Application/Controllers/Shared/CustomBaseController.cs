using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LocalStore.Services.Interfaces;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Infra.Services.BlobStorage.Implementations;
namespace LocalStore.Application.Controllers.Shared
{
    [Route("api/")]
    [ApiController]
    public class CustomBaseController<T> : ControllerBase where T : class
    {
        protected IConfiguration Configuration { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IServicesLayer Services { get; }
        protected Logger<T> Logger { get; }
        protected IBlobStorageService BlobStorage { get; }



        public CustomBaseController(IServiceProvider serviceProvider)
        {
            Configuration = serviceProvider.GetService<IConfiguration>();
            UserManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            SignInManager = serviceProvider.GetService<SignInManager<IdentityUser>>();
            Services = serviceProvider.GetService<IServicesLayer>();
            BlobStorage = serviceProvider.GetService<IBlobStorageService>();
            Logger = serviceProvider.GetService<Logger<T>>();    
        }

    }
}
