using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LucroImpresso.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMaquinas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaquinaId",
                table: "Orcamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Maquinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsumoWatts = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CustoKwh = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_MaquinaId",
                table: "Orcamentos",
                column: "MaquinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orcamentos_Maquinas_MaquinaId",
                table: "Orcamentos",
                column: "MaquinaId",
                principalTable: "Maquinas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orcamentos_Maquinas_MaquinaId",
                table: "Orcamentos");

            migrationBuilder.DropTable(
                name: "Maquinas");

            migrationBuilder.DropIndex(
                name: "IX_Orcamentos_MaquinaId",
                table: "Orcamentos");

            migrationBuilder.DropColumn(
                name: "MaquinaId",
                table: "Orcamentos");
        }
    }
}
