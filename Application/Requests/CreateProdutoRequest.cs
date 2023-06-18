using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Domain.Model;
using LocalStore.Domain.Enum;
namespace LocalStore.Application.Requests
{

    public class CreateProdutoRequest 
    {
        public int EstabelecimentoId { get; set; }
        public string Nome { get; set; }
        public string? Marca { get; set; }
        public double Peso { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime? VencimentoEm { get; set; }
        public Double QuantidadeEstoque { get; set; }
        public double ValorUnitario { get; set; }
        public double? ValorComDesconto { get; set; }
        public string? Lote { get; set; }
        public string? Observacao { get; set; }
        public string? NomeArquivoImagem { get; set; }
        public string? ExtensaoArquivoImagem { get; set; }
        public string? ConteudoArquivoImagem { get; set; }
        public FormaDeVendaProduto VendidoPor { get; set; }
        public string UnidadeMedida { get; set; }

        public Produto ToProduto()
        {
            return new Produto()
            {
                Nome = Nome,
                Observacao = Observacao,
                Peso = Peso,
                VencimentoEm = VencimentoEm,
                QuantidadeEstoque = QuantidadeEstoque,
                ValorUnitario = ValorUnitario,
                ValorComDesconto = ValorComDesconto,
                EstabelecimentoId = EstabelecimentoId,
                Lote = Lote,
                Marca = Marca,
                VendidoPor = VendidoPor,
                UnidadeMedida = UnidadeMedida,
                Categoria = Categoria,

            };
        }

    }
}
