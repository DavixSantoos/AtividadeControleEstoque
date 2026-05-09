namespace ControleEstoque.API.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public decimal Total => Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        public List<ItemPedidoDto> Itens { get; set; } = new();
    }

    public class CriarPedidoDto
    {
        // remover quando tiver o JWT
        public int ClienteId { get; set; }
        public List<CriarItemPedidoDto> Itens { get; set; } = new();
    }

    public class CriarItemPedidoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }

    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int? ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
    }

    public class DetalhesPedidoDto
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ClienteId { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedidoDto> Itens { get; set; } = new();
    }
}