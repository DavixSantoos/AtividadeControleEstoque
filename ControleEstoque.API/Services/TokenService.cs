using Microsoft.IdentityModel.Tokens;
using ControleEstoque.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControleEstoque.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Alterado de 'Id Id' para 'Usuario user' (ou o nome da sua classe de modelo)
        public string GenerateToken(string id, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Verifica se a chave existe no appsettings.json para não dar erro de nulo
            var keyString = _configuration["Jwt:Key"] ?? "r8fGMkVlDZwA5reYsFqMoeHyKHvc6iYW75R2Sm3JP0PbD4pWB6WurgUy73TH1Ur0";
            var key = Encoding.ASCII.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Email, email)


                }),
                Expires = DateTime.UtcNow.AddHours(2),
           
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}