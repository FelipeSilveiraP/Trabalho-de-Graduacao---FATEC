using System.ComponentModel.DataAnnotations;

namespace LucroImpresso.Web.Models
{
    public class Maquina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da máquina é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A potência em Watts é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A potência deve ser maior que zero.")]
        public decimal ConsumoWatts { get; set; }

        [Required(ErrorMessage = "A taxa de kWh é obrigatória.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O custo do kWh deve ser maior que zero.")]
        public decimal CustoKwh { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
