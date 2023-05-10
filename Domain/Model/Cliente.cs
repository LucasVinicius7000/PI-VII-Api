using System.ComponentModel.DataAnnotations.Schema;

namespace LocalStore.Domain.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public List<Pedido> Pedidos { get; set; }
        
    }
}
