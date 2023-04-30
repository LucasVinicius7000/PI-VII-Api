using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalStore.Domain.Model
{
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string? Descricao { get; set; }
        public string Endereco { get; set; }
        public string NomeProprietario { get; set; }
        public string CPFProprietario { get; set; }
        public string Telefone { get; set; }
        public string? UrlLogoPerfil { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        [NotMapped]
        public Double DistanciaEstabelecimentoUsuario { get; set; } 
        public Boolean Aprovado { get; set; }

    }
}
