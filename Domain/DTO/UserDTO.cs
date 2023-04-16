using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace LocalStore.Domain.DTO
{

    public class UserDTO : IdentityUser
    {
        public string Senha { get; set; }
    }
}
