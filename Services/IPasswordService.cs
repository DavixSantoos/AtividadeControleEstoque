namespace ControleEstoque.API.Services
{
    public interface IPasswordService
    {
        string HashPassword(string senha);
        bool VerifyPassword(string senha, string hash);
    }
}