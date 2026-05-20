using System.ComponentModel.DataAnnotations;

namespace LucroImpresso.Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O usuário é obrigatório.")]
        [MaxLength(50)]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MaxLength(100)]
        public string Senha { get; set; } = string.Empty;
    }
}
