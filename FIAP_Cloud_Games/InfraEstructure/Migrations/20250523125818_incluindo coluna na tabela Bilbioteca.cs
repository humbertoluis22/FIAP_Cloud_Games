using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraEstructure.Migrations
{
    /// <inheritdoc />
    public partial class incluindocolunanatabelaBilbioteca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstaEmprestado",
                table: "Biblioteca",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstaEmprestado",
                table: "Biblioteca");
        }
    }
}
