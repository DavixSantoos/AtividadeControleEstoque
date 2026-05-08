using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDto> RegistrarClienteAsync(CriarClienteDto dto)
        {
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = dto.Senha, // Sem criptografia nesta etapa inicial
                CPF = dto.CPF,
                Perfil = PerfilUsuario.Cliente
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return MapearParaDto(cliente);
        }

        public async Task<UsuarioDto> RegistrarCaixaAsync(CriarCaixaDto dto)
        {
            var caixa = new Caixa
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = dto.Senha,
                Turno = dto.Turno,
                Perfil = PerfilUsuario.Caixa
            };

            _context.Caixas.Add(caixa);
            await _context.SaveChangesAsync();
            return MapearParaDto(caixa);
        }

        public async Task<UsuarioDto> RegistrarGerenteAsync(CriarGerenteDto dto)
        {
            var gerente = new Gerente
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = dto.Senha,
                Setor = dto.Setor,
                Perfil = PerfilUsuario.Gerente
            };

            _context.Gerentes.Add(gerente);
            await _context.SaveChangesAsync();
            return MapearParaDto(gerente);
        }

        public async Task<IEnumerable<UsuarioDto>> ListarTodosUsuariosAsync()
        {
            var usuarios = await _context.Usuarios.AsNoTracking().ToListAsync();
            return usuarios.Select(MapearParaDto);
        }

        public async Task<UsuarioDto?> ObterUsuarioPorEmailAsync(string email)
        {
            var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            return usuario != null ? MapearParaDto(usuario) : null;
        }

        private static UsuarioDto MapearParaDto(Usuario usuario)
        {
            var dto = new UsuarioDto
            {
                Id = usuario.Id, Nome = usuario.Nome, Email = usuario.Email, Perfil = usuario.Perfil.ToString()
            };
            if (usuario is Cliente cliente) dto.CPF = cliente.CPF;
            if (usuario is Caixa caixa) dto.Turno = caixa.Turno;
            if (usuario is Gerente gerente) dto.Setor = gerente.Setor;
            return dto;
        }
    }
}