using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpAcademico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelacionamentoMovimentacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId1",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesEstoque_ProdutoId1",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropColumn(
                name: "ProdutoId1",
                table: "MovimentacoesEstoque");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProdutoId1",
                table: "MovimentacoesEstoque",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_ProdutoId1",
                table: "MovimentacoesEstoque",
                column: "ProdutoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId1",
                table: "MovimentacoesEstoque",
                column: "ProdutoId1",
                principalTable: "Produtos",
                principalColumn: "Id");
        }
    }
}
