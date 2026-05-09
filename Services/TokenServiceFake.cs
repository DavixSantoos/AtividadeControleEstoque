using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Services
{
    public class TokenServiceFake : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenServiceFake(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            // Chave secreta do appsettings.json ("Jwt:Key")
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims que vão dentro do token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nome", usuario.Nome),
                new Claim("perfil", usuario.Perfil.ToString())
            };

            // Monta o token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            // Converte para string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}