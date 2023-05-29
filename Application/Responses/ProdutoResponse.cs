using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Domain.Enum;
using LocalStore.Domain.Model;


namespace LocalStore.Application.Responses
{

    public class ProdutoResponse 
    {
        public int Id { get; set; }
        public int EstabelecimentoId { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public double Peso { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime? VencimentoEm { get; set; }
        public int QuantidadeEstoque { get; set; }
        public double ValorUnitario { get; set; }
        public double? ValorComDesconto { get; set; }
        public string? Lote { get; set; }
        public string Observacao { get; set; }
        public string UrlImagem { get; set; }

        public ProdutoResponse(Produto produto)
        {
            this.Nome = produto.Nome;
            this.EstabelecimentoId = produto.EstabelecimentoId;
            this.Id = produto.Id;
            this.Lote = produto.Lote;
            this.Marca = produto.Marca;
            this.Peso = produto.Peso;
            this.ValorUnitario = produto.ValorUnitario;
            this.ValorComDesconto = produto.ValorComDesconto;
            this.UrlImagem = produto.UrlImagem;
            this.Observacao = produto.Observacao;
            this.Categoria = produto.Categoria;
            this.VencimentoEm = produto.VencimentoEm;
            this.QuantidadeEstoque = produto.QuantidadeEstoque;
        }

    }
}
