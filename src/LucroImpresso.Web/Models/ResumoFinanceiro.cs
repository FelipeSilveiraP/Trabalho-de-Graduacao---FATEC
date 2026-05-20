using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LucroImpresso.Web.Models
{
    public class ResumoFinanceiro
    {
        [Key]
        public int Id { get; set; } // Será sempre 1

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceitasAcumuladas { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DespesasMaterialAcumuladas { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DespesasEnergiaAcumuladas { get; set; } = 0;
    }
}
