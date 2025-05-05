using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi_Coris.Migrations
{
    /// <inheritdoc />
    public partial class alter_personal_access_token_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "personal_access_tokens",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "personal_access_tokens",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "personal_access_tokens_token_unique",
                table: "personal_access_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "personal_access_tokens_tokenable_type_tokenable_id_index",
                table: "personal_access_tokens",
                columns: new[] { "tokenable_type", "tokenable_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "personal_access_tokens_token_unique",
                table: "personal_access_tokens");

            migrationBuilder.DropIndex(
                name: "personal_access_tokens_tokenable_type_tokenable_id_index",
                table: "personal_access_tokens");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "personal_access_tokens",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "personal_access_tokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
