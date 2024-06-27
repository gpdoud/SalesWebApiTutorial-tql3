using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebApiTutorial.Migrations
{
    /// <inheritdoc />
    public partial class addedtotaltoorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orderlines_Orders_OrderId",
                table: "Orderlines");

 

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(11,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Orderlines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orderlines_Orders_OrderId",
                table: "Orderlines",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orderlines_Orders_OrderId",
                table: "Orderlines");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Orderlines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");



            migrationBuilder.AddForeignKey(
                name: "FK_Orderlines_Orders_OrderId",
                table: "Orderlines",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
