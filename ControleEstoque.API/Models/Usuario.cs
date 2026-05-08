using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.API.Models
{
    public enum PerfilUsuario { Cliente, Caixa, Gerente }

    public abstract class Usuario // abstract, impede instancia da classe
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; } // aplicar tamanho mínimo

        [Required]
        public PerfilUsuario Perfil { get; set; }
    }
}
