using ControleEstoque.API.Models; // vai precisar do modelo Usuario

namespace ControleEstoque.API.Services
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}