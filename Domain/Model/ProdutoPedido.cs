using System.ComponentModel.DataAnnotations.Schema;
using LocalStore.Domain.Enum;

namespace LocalStore.Domain.Model
{
    public class ProdutoPedido 
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoOriginalId { get; set; }
        [NotMapped]
        public Pedido Pedido { get; set; }
        public string Nome { get; set; }
        public string? Marca { get; set; }
        public double? Peso { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime? VencimentoEm { get; set; }
        public double? QuantidadePedido { get; set; }
        public double ValorUnitario { get; set; }
        public double? ValorComDesconto { get; set; }
        public string? Lote { get; set; }
        public string? Observacao { get; set; }
        public string? UrlImagem { get; set; }
        public FormaDeVendaProduto VendidoPor { get; set; }
        public string UnidadeMedida { get; set; }
        public bool Removed { get; set; }
        [NotMapped]
        public double? QuantidadeMax { get; set; } = 0.0;
    }
}
