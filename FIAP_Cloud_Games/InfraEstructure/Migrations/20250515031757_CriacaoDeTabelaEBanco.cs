using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraEstructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoDeTabelaEBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Senha = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Senha = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataInscricao = table.Column<DateTime>(type: "datetime", nullable: false),
                    TentativasErradas = table.Column<int>(type: "int", nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Jogo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAdmin = table.Column<int>(type: "int", nullable: false),
                    NomeJogo = table.Column<string>(type: "varchar(50)", nullable: false),
                    Genero = table.Column<string>(type: "varchar(20)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(200)", nullable: false),
                    Desenvolvedor = table.Column<string>(type: "varchar(50)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdminID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Jogo_Admin_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Admin",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_AdminID",
                table: "Jogo",
                column: "AdminID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jogo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Admin");
        }
    }
}
