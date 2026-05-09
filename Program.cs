using ControleEstoque.API.Data;
using ControleEstoque.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer; // novo: using do JWT
using Microsoft.IdentityModel.Tokens;               // novo: using de chaves
using System.Text;                                   // novo: para Encoding

var builder = WebApplication.CreateBuilder(args);

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ===== REGISTRO DOS SERVIÇOS (todos juntos) =====
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IContaReceberService, ContaReceberService>();
builder.Services.AddScoped<IPasswordService, PasswordServiceFake>();
builder.Services.AddScoped<ITokenService, TokenServiceFake>();

// ===== CONFIGURAÇÃO DOS CONTROLLERS E JSON =====
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// ===== CONFIGURAÇÃO DO SWAGGER =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// AQUI LIGAMOS A AUTENTICAÇÃO JWT
// ==========================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ===== ATENÇÃO: ORDEM OBRIGATÓRIA =====
app.UseAuthentication();  // <-- verifica a pulseira (token)
app.UseAuthorization();   // <-- decide se pode entrar na sala

app.MapControllers();

app.Run();