namespace ControleEstoque.API.Services
{
    public class PasswordServiceFake : IPasswordService
    {
        // No fake, guardamos a senha pura (só para teste!)
        public string HashPassword(string senha)
        {
            return senha; // sem criptografia
        }

        public bool VerifyPassword(string senha, string hash)
        {
            // Compara a senha com o que temos guardado (que é a própria senha)
            return senha == hash;
        }
    }
}