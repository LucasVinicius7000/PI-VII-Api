using System.ComponentModel.DataAnnotations.Schema;
using LocalStore.Domain.Enum;

namespace LocalStore.Domain.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public int EstabelecimentoId { get; set; }
        [NotMapped]
        public Estabelecimento? Estabelecimento { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }   
        public double Peso { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime? VencimentoEm { get; set; }
        public int QuantidadeEstoque { get; set; }
        public double ValorUnitario { get; set; }
        public double? ValorComDesconto { get; set; }
        public string? Lote { get; set; }
        public string? Observacao { get; set; }
        public string UrlImagem { get; set; }
    }
}
