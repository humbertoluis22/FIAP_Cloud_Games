using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraEstructure.Migrations
{
    /// <inheritdoc />
    public partial class bibliotecamigrationcascate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biblioteca_Usuario_UsuarioID",
                table: "Biblioteca");

            migrationBuilder.AddForeignKey(
                name: "FK_Biblioteca_Usuario_UsuarioID",
                table: "Biblioteca",
                column: "UsuarioID",
                principalTable: "Usuario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biblioteca_Usuario_UsuarioID",
                table: "Biblioteca");

            migrationBuilder.AddForeignKey(
                name: "FK_Biblioteca_Usuario_UsuarioID",
                table: "Biblioteca",
                column: "UsuarioID",
                principalTable: "Usuario",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
