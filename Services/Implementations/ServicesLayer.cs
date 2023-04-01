using LocalStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Services.Implementations
{
    public class ServicesLayer : IServicesLayer
    {
        #region Propriedades privadas

        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        #endregion

        #region Propriedades públicas

        public IConfiguration Configuration => _configuration;
        public UserManager<IdentityUser> UserManager => _userManager;
        public RoleManager<IdentityRole> RoleManager => _roleManager;
        public SignInManager<IdentityUser> SignInManager => _signInManager;
        public UserServices User { get; }

        #endregion

        public ServicesLayer
        (
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            User ??= new UserServices(this);
        }
        
    }
}
