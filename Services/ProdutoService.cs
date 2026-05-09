using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _context;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
        {
            return await _context.Produtos
                .Include(p => p.Fornecedor)
                .AsNoTracking()
                .Select(p => new ProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    QuantidadeEstoque = p.QuantidadeEstoque,
                    FornecedorId = p.FornecedorId,
                    Fornecedor = p.Fornecedor != null ? new FornecedorDto 
                    { 
                        Id = p.Fornecedor.Id, 
                        NomeFantasia = p.Fornecedor.NomeFantasia, 
                        CNPJ = p.Fornecedor.CNPJ 
                    } : null
                })
                .ToListAsync();
        }

        public async Task<ProdutoDto?> ObterPorIdAsync(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Fornecedor)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) return null;

            return new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                FornecedorId = produto.FornecedorId,
                Fornecedor = produto.Fornecedor != null ? new FornecedorDto 
                { 
                    Id = produto.Fornecedor.Id, 
                    NomeFantasia = produto.Fornecedor.NomeFantasia, 
                    CNPJ = produto.Fornecedor.CNPJ 
                } : null
            };
        }

        public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
        {
            var fornecedorExiste = await _context.Fornecedores.AnyAsync(f => f.Id == dto.FornecedorId);
            if (!fornecedorExiste)
            {
                throw new ArgumentException("O fornecedor informado não existe.");
            }

            var produto = new Produto
            {
                Nome = dto.Nome,
                Preco = dto.Preco, // Garante cadastro do preço do produto corretamente
                QuantidadeEstoque = dto.QuantidadeEstoque,
                FornecedorId = dto.FornecedorId
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                FornecedorId = produto.FornecedorId
            };
        }

        public async Task AtualizarAsync(AtualizarProdutoDto dto)
        {
            var produto = await _context.Produtos.FindAsync(dto.Id);
            if (produto != null)
            {
                var fornecedorExiste = await _context.Fornecedores.AnyAsync(f => f.Id == dto.FornecedorId);
                if (!fornecedorExiste)
                {
                    throw new ArgumentException("O fornecedor informado não existe.");
                }

                produto.Nome = dto.Nome;
                produto.Preco = dto.Preco;
                produto.QuantidadeEstoque = dto.QuantidadeEstoque;
                produto.FornecedorId = dto.FornecedorId;

                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }
    }
}