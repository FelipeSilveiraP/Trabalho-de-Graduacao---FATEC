using System.ComponentModel.DataAnnotations;

namespace LucroImpresso.Web.Models
{
    public class Material : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do material é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        public string Cor { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O peso do rolo deve ser maior que zero.")]
        public decimal PesoRoloGramas { get; set; }

        [Required(ErrorMessage = "O valor pago é obrigatório.")]
        public decimal ValorPago { get; set; }

        [Required(ErrorMessage = "O saldo é obrigatório.")]
        public decimal SaldoGramas { get; set; }

        // Mapeamento Direto da RN001: Preço do material é calculado por grama
        public decimal PrecoPorGrama => PesoRoloGramas > 0 ? ValorPago / PesoRoloGramas : 0;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SaldoGramas > PesoRoloGramas)
            {
                yield return new ValidationResult(
                    "O saldo atual não pode ser superior ao peso original do rolo.",
                    new[] { nameof(SaldoGramas) });
            }
        }
    }
}