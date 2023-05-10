using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
namespace LocalStore.Application.Responses
{
    public class ClienteResponse
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public ClienteResponse() { }

        public ClienteResponse(Cliente cliente, User userDto)
        {
            this.Email = userDto.Email;
            this.Telefone = userDto.PhoneNumber;
            this.Name = cliente.Nome;
            this.UserName = userDto.UserName;    
        }

    }
}
