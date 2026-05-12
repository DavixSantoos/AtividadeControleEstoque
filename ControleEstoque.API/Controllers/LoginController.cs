using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ControleEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public LoginController(
            IUsuarioService usuarioService,
            IPasswordService passwordService,
            ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        [HttpPost] 
        public IActionResult Login([FromBody] UsuarioLoginDto login)
        {
            try
            {
                var usuario = _usuarioService.BuscarPorEmail(login.Email);

                if (usuario == null)
                {
                    return Unauthorized("Usuário não encontrado");
                }

                var senhaValida = _passwordService.VerificarSenha(
                    login.Senha,
                    usuario.Senha
                );

                if (!senhaValida)
                {
                    return Unauthorized("Senha inválida");
                }

                var token = _tokenService.GerarToken(usuario);

                var retorno = new RetornoLoginDto
                {
                    Token = token
                };

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}