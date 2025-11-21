using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cafeservellocontroler.Migrations
{
    /// <inheritdoc />
    public partial class loginUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DataAtualizacaoCadastro", "DataCadastro", "Senha" },
                values: new object[] { new DateTime(2025, 11, 21, 16, 53, 43, 379, DateTimeKind.Local).AddTicks(9883), new DateTime(2025, 11, 21, 16, 53, 43, 379, DateTimeKind.Local).AddTicks(9876), "7c4a8d09ca3762af61e59520943dc26494f8941b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "DataAtualizacaoCadastro", "DataCadastro", "Senha" },
                values: new object[] { new DateTime(2025, 11, 21, 16, 48, 13, 549, DateTimeKind.Local).AddTicks(6777), new DateTime(2025, 11, 21, 16, 48, 13, 549, DateTimeKind.Local).AddTicks(6769), "123456" });
        }
    }
}
