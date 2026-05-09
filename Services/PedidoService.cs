using ControleEstoque.API.Data;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> CriarPedidoAsync(int clienteId, List<ItemPedido> itens)
        {
            foreach (var item in itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto == null) throw new Exception($"Produto {item.ProdutoId} não encontrado no estoque.");

                // Corrige a falha onde o pedido gravava o item sem preço
                item.PrecoUnitario = produto.Preco;

                // Reduz do estoque dinamicamente 
                produto.QuantidadeEstoque -= item.Quantidade;
            }

            var pedido = new Pedido()
            {
                ClienteId = clienteId,
                DataPedido = DateTime.Now,
                Status = "Aberto",
                Itens = itens
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<IEnumerable<Pedido>> ListarPedidosDoClienteAsync(int clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Where(p => p.ClienteId == clienteId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPedidoComDetalhesAsync(int pedidoId)
        {
            // Trazendo as dependências corretamente com Include
            return await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }
    }
}
