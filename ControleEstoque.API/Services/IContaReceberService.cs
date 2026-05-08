using ControleEstoque.API.DTOs;

namespace ControleEstoque.API.Services
{
    public interface IContaReceberService
    {
        Task<IEnumerable<ContaReceberDto>> ObterTodosAsync();
        Task<ContaReceberDto?> ObterPorIdAsync(int id);
        Task<ContaReceberDto> CriarAsync(CriarContaReceberDto dto);
        Task AtualizarAsync(AtualizarContaReceberDto dto);
        Task RemoverAsync(int id);
    }
}