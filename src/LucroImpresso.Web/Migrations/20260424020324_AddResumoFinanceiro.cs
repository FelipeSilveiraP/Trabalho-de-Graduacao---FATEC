using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LucroImpresso.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddResumoFinanceiro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResumoFinanceiro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceitasAcumuladas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DespesasMaterialAcumuladas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DespesasEnergiaAcumuladas = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumoFinanceiro", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResumoFinanceiro");
        }
    }
}
