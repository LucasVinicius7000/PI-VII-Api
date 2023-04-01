﻿using Microsoft.AspNetCore.Identity;

namespace LocalStore.Services.Interfaces
{
    public interface IServicesLayer
    {
        IConfiguration Configuration { get; }
        UserManager<IdentityUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<IdentityUser> SignInManager { get; }
        UserServices User { get; }
    }
}