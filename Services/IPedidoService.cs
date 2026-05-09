using ControleEstoque.API.Models;

namespace ControleEstoque.API.Services
{
    public interface IPedidoService
    {
        Task<Pedido?> ObterPedidoComDetalhesAsync(int pedidoId);
        Task<IEnumerable<Pedido>> ListarPedidosDoClienteAsync(int clienteId);
        Task<Pedido> CriarPedidoAsync (int clienteId, List<ItemPedido> itens);
    }
}
