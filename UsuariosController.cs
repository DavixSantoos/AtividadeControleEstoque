using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public UsuariosController(
            IUsuarioService usuarioService,
            IPasswordService passwordService,
            ITokenService tokenService,
            AppDbContext context)
        {
            _usuarioService = usuarioService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPost("registrar-cliente")]
        public async Task<IActionResult> RegistrarCliente([FromBody] CriarClienteDto dto)
        {
            var novoCliente = await _usuarioService.RegistrarClienteAsync(dto);
            return Ok(novoCliente);
        }

        [HttpPost("registrar-caixa")]
        public async Task<IActionResult> RegistrarCaixa([FromBody] CriarCaixaDto dto)
        {
            var novoCaixa = await _usuarioService.RegistrarCaixaAsync(dto);
            return Ok(novoCaixa);
        }

        [HttpPost("registrar-gerente")]
        public async Task<IActionResult> RegistrarGerente([FromBody] CriarGerenteDto dto)
        {
            var novoGerente = await _usuarioService.RegistrarGerenteAsync(dto);
            return Ok(novoGerente);
        }

        // ========== MÉTODO DE LOGIN ==========
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
        {
            // Busca o usuário pelo email direto do banco
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return Unauthorized("Email ou senha inválidos.");

            // Verifica a senha
            bool senhaValida = _passwordService.VerifyPassword(dto.Senha, usuario.SenhaHash);
            if (!senhaValida)
                return Unauthorized("Email ou senha inválidos.");

            // Gera o token
            var token = _tokenService.GerarToken(usuario);

            // Retorna o token + dados básicos
            return Ok(new LoginResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil.ToString()
            });
        }
    }
}