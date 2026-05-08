using ControleEstoque.API.DTOs;

namespace ControleEstoque.API.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
        Task<ProdutoDto?> ObterPorIdAsync(int id);
        Task<ProdutoDto> CriarAsync(CriarProdutoDto dto);
        Task AtualizarAsync(AtualizarProdutoDto dto);
        Task RemoverAsync(int id);
    }
}