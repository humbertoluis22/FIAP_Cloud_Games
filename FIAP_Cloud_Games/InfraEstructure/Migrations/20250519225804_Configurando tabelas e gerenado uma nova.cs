using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraEstructure.Migrations
{
    /// <inheritdoc />
    public partial class Configurandotabelasegerenadoumanova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Admin_AdminID",
                table: "Jogo");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_AdminID",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "AdminID",
                table: "Jogo");

            migrationBuilder.CreateTable(
                name: "Biblioteca",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JogoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    JogoEmprestado = table.Column<bool>(type: "bit", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteca", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Biblioteca_Jogo_JogoID",
                        column: x => x.JogoID,
                        principalTable: "Jogo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Biblioteca_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_IdAdmin",
                table: "Jogo",
                column: "IdAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_JogoID",
                table: "Biblioteca",
                column: "JogoID");

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteca_UsuarioID",
                table: "Biblioteca",
                column: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Admin_IdAdmin",
                table: "Jogo",
                column: "IdAdmin",
                principalTable: "Admin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Admin_IdAdmin",
                table: "Jogo");

            migrationBuilder.DropTable(
                name: "Biblioteca");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_IdAdmin",
                table: "Jogo");

            migrationBuilder.AddColumn<int>(
                name: "AdminID",
                table: "Jogo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_AdminID",
                table: "Jogo",
                column: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Admin_AdminID",
                table: "Jogo",
                column: "AdminID",
                principalTable: "Admin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
