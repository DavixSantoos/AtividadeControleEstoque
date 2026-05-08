using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.API.Models
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string NomeFantasia { get; set; }

        [Required, StringLength(14)]
        public string CNPJ { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}
