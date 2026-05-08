using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpAcademico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMovimentacaoRelacionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId",
                table: "MovimentacoesEstoque");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId",
                table: "MovimentacoesEstoque",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId",
                table: "MovimentacoesEstoque");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Produtos_ProdutoId",
                table: "MovimentacoesEstoque",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
