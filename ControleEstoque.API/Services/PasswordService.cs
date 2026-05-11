

namespace ControleEstoque.API.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }
        public bool VerifyPassword(string senha, string senhaHashed)
        {
            return BCrypt.Net.BCrypt.Verify(senha, senhaHashed);
        }
    }
}