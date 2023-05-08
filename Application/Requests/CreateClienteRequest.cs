using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;

namespace LocalStore.Application.Requests
{
    public class CreateClienteRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }

        public Cliente ToCliente()
        {
            return new Cliente()
            {
                CPF = CPF,
                Nome = Name,

            };
        }

        public User ToUserDTO()
        {
            return new User()
            {
                UserName = UserName,
                Email = Email,
                Senha = Senha,
            };
        }
    }
}
