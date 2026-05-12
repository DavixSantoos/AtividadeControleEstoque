using ControleEstoque.API.Data;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

// 1. PRIMEIRO criamos o builder
var builder = WebApplication.CreateBuilder(args);

// 2. DEPOIS configuramos a chave e o JWT (usando o builder que já existe)
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "ChaveSegurancaDePeloMenos32Caracteres");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 3. Configurações de Banco e outros Services
builder.Services.AddDbContext<AppDbContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IContaReceberService, ContaReceberService>();
builder.Services.AddScoped<ITokenService, TokenService>(); // Seu serviço aqui!

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. BUILD (Transforma o builder no app)
var app = builder.Build();

// 5. Middlewares (Configuração de execução)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// A ordem aqui é sagrada: Autenticação ANTES de Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();