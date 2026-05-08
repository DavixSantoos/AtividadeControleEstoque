using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstoque.API.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required, StringLength(20)]
        public string Status { get; set; } // aberto, fechado, supspenso...

        [ForeignKey("Cliente")] // Cliente que fez o pedido
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("Caixa")] // Caixa que fecha o pedido
        public int? CaixaId { get; set; }
        public Caixa Caixa { get; set; }

        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();        
    }
}
