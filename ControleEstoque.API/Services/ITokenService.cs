namespace ControleEstoque.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(string Id, string Email);
       
    }
}
