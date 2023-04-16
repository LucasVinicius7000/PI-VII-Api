using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Domain.Model;

namespace LocalStore.Application.Responses
{

    public class EstabelecimentoUsuarioResponse 
    {
        public string UserName { get; set; }
        public string Email { get; set; }
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

        public EstabelecimentoUsuarioResponse(Estabelecimento estabelecimento)
        {
            this.CPFProprietario = estabelecimento.CPFProprietario;
            this.NomeProprietario = estabelecimento.NomeProprietario;
            this.Longitude = estabelecimento.Longitude;
            this.Latitude = estabelecimento.Latitude;
            this.CNPJ = estabelecimento.CNPJ;
            this.Descricao = estabelecimento.Descricao;
            this.Endereco = estabelecimento.Endereco;
            this.Telefone = estabelecimento.Telefone;
            this.UrlLogoPerfil = estabelecimento.UrlLogoPerfil;
            this.Email = estabelecimento.Email;
            this.RazaoSocial = estabelecimento.RazaoSocial;
            this.NomeFantasia = estabelecimento.NomeFantasia;
        }

    }
}
