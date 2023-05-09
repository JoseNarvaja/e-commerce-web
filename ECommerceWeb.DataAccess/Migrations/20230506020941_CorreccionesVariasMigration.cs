using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionesVariasMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PrecioIndividual",
                table: "PedidoDetalle",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "PedidoDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoDetalle_IdProducto",
                table: "PedidoDetalle",
                column: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoDetalle_Producto_IdProducto",
                table: "PedidoDetalle",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoDetalle_Producto_IdProducto",
                table: "PedidoDetalle");

            migrationBuilder.DropIndex(
                name: "IX_PedidoDetalle_IdProducto",
                table: "PedidoDetalle");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "PedidoDetalle");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Pedido");

            migrationBuilder.AlterColumn<int>(
                name: "PrecioIndividual",
                table: "PedidoDetalle",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
