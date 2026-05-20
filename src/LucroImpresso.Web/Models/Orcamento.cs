using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LucroImpresso.Web.Models
{
    public enum StatusOrcamento
    {
        Pendente,
        EmImpressao,
        Concluido,
        Falha
    }

    public class Orcamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string TelefoneCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        public string NomeProjeto { get; set; } = string.Empty;

        public string NomePeca { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Link STL é obrigatório. (Coloque 'N/A' se não houver)")]
        public string LinkArquivoStl { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O peso deve ser maior que zero.")]
        public double PesoGramas { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O tempo deve ser pelo menos 1 hora.")]
        public int TempoHoras { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MargemLucroPercentual { get; set; }

        public StatusOrcamento Status { get; set; } = StatusOrcamento.Pendente;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsumoMaquinaW { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoKwh { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoMaoDeObraHora { get; set; }

        public int? MaquinaId { get; set; }

        [ForeignKey("MaquinaId")]
        public Maquina? Maquina { get; set; }

        // Chave Estrangeira
        [Required(ErrorMessage = "Selecione um material.")]
        public int MaterialId { get; set; }

        [ForeignKey("MaterialId")]
        public Material? Material { get; set; }

        // RN001 + Cálculo de Margem de Lucro
        [NotMapped]
        public decimal CustoFinal
        {
            get
            {
                if (Material == null) return 0;
                decimal custoMaterial = (decimal)PesoGramas * Material.PrecoPorGrama;
                decimal custoEnergia = (ConsumoMaquinaW / 1000m) * TempoHoras * CustoKwh;
                decimal custoMaoDeObra = TempoHoras * CustoMaoDeObraHora;
                return (custoMaterial + custoEnergia + custoMaoDeObra) * (1 + (MargemLucroPercentual / 100));
            }
        }
    }
}
