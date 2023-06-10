using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using LocalStore.Domain.Enum;

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
        public string? Endereco { get; set; }
        public string? NomeProprietario { get; set; }
        public string? CPFProprietario { get; set; }
        public string Telefone { get; set; }
        public string? UrlLogoPerfil { get; set; }
        public string? UrlAlvaraFuncionamento { get; set; } 
        public string? FormasPagamentoAceitas { get; set; }
        public double TaxaMinimaEntrega { get; set; }
        public double TaxaKmRodado { get; set; }
        public MetodoCompra MetodoCompra { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        [NotMapped]
        public Double DistanciaEstabelecimentoUsuario { get; set; } 
        public StatusAprovacao Aprovado { get; set; }
        [NotMapped]
        public List<Produto> Produtos { get; set; }
        [NotMapped]
        public List<Pedido> Pedidos { get; set; }

    }
}
