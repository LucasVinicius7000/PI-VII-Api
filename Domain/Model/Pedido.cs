using System.ComponentModel.DataAnnotations.Schema;
using LocalStore.Domain.Enum;
namespace LocalStore.Domain.Model
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        [NotMapped]
        public Cliente Cliente { get; set; }
        public int EstabelecimentoId { get; set; }
        [NotMapped]
        public Estabelecimento Estabelecimento { get; set; }
        public FormaPagamento? FormaPagamento { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public List<ProdutoPedido> ProdutosPedidos { get; set; }
        public Boolean IsProdutoAtual {get;set;}
    }
}
