using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace task.app.Migrations
{
    public partial class mg223 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Connections_ConnectionsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Connections_UserId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ConnectionsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConnectionsId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserId",
                table: "Connections",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Connections_UserId",
                table: "Connections");

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectionsId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserId",
                table: "Connections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConnectionsId",
                table: "AspNetUsers",
                column: "ConnectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Connections_ConnectionsId",
                table: "AspNetUsers",
                column: "ConnectionsId",
                principalTable: "Connections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
