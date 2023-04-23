using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;

namespace LocalStore.Application.Requests
{
    public class CriarEstabelecimentoUsuarioRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string Descricao { get; set; }
        public string Endereco { get; set; }
        public string NomeProprietario { get; set; }
        public string CPFProprietario { get; set; }
        public string Telefone { get; set; }
        public string UrlLogoPerfil { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }

        public Estabelecimento ToEstabelecimento()
        {
            return new Estabelecimento()
            {
                NomeFantasia = this.NomeFantasia,
                NomeProprietario = this.NomeProprietario,
                RazaoSocial = this.RazaoSocial,
                CNPJ = this.CNPJ,
                Descricao = this.Descricao,
                Endereco = this.Endereco,
                CPFProprietario = this.CPFProprietario,
                Telefone = this.Telefone,
                UrlLogoPerfil = this.UrlLogoPerfil,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
            };

        }

        public User ToUserDTO()
        {
            return new User()
            {
                UserName = this.UserName,
                Email = this.Email,
                Senha = this.Senha,
            };
        }

    }
}
