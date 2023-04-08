using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Controllers.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController<T> : ControllerBase where T : class
    {
        protected IConfiguration Configuration { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected Logger<T> Logger { get; }

        public CustomBaseController(IServiceProvider serviceProvider)
        {
            Configuration = serviceProvider.GetService<IConfiguration>();
            UserManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            Logger = serviceProvider.GetService<Logger<T>>();
        }

    }
}
