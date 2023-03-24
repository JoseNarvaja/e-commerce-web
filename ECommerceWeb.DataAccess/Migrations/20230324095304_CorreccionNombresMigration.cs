using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionNombresMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompras_AspNetUsers_IdUsuario",
                table: "CarritoCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompras_Producto_IdProducto",
                table: "CarritoCompras");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_AspNetUsers_IdUsuario",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categorias_IdCategoria",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "DetallePedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritoCompras",
                table: "CarritoCompras");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Pedido");

            migrationBuilder.RenameTable(
                name: "Categorias",
                newName: "Categoria");

            migrationBuilder.RenameTable(
                name: "CarritoCompras",
                newName: "CarritoCompra");

            migrationBuilder.RenameColumn(
                name: "marca",
                table: "Producto",
                newName: "Marca");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_IdUsuario",
                table: "Pedido",
                newName: "IX_Pedido_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_CarritoCompras_IdUsuario",
                table: "CarritoCompra",
                newName: "IX_CarritoCompra_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_CarritoCompras_IdProducto",
                table: "CarritoCompra",
                newName: "IX_CarritoCompra_IdProducto");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "IdPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria",
                column: "IdCategoria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritoCompra",
                table: "CarritoCompra",
                column: "IdCarritoCompra");

            migrationBuilder.CreateTable(
                name: "PedidoDetalle",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioIndividual = table.Column<int>(type: "int", nullable: false),
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoDetalle", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_PedidoDetalle_Pedido_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedido",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoDetalle_IdPedido",
                table: "PedidoDetalle",
                column: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoCompra_AspNetUsers_IdUsuario",
                table: "CarritoCompra",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoCompra_Producto_IdProducto",
                table: "CarritoCompra",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_AspNetUsers_IdUsuario",
                table: "Pedido",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categoria_IdCategoria",
                table: "Producto",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompra_AspNetUsers_IdUsuario",
                table: "CarritoCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompra_Producto_IdProducto",
                table: "CarritoCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_AspNetUsers_IdUsuario",
                table: "Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categoria_IdCategoria",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "PedidoDetalle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarritoCompra",
                table: "CarritoCompra");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "Pedidos");

            migrationBuilder.RenameTable(
                name: "Categoria",
                newName: "Categorias");

            migrationBuilder.RenameTable(
                name: "CarritoCompra",
                newName: "CarritoCompras");

            migrationBuilder.RenameColumn(
                name: "Marca",
                table: "Producto",
                newName: "marca");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_IdUsuario",
                table: "Pedidos",
                newName: "IX_Pedidos_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_CarritoCompra_IdUsuario",
                table: "CarritoCompras",
                newName: "IX_CarritoCompras_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_CarritoCompra_IdProducto",
                table: "CarritoCompras",
                newName: "IX_CarritoCompras_IdProducto");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "IdPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorias",
                table: "Categorias",
                column: "IdCategoria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarritoCompras",
                table: "CarritoCompras",
                column: "IdCarritoCompra");

            migrationBuilder.CreateTable(
                name: "DetallePedido",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioIndividual = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedido", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_DetallePedido_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedido_IdPedido",
                table: "DetallePedido",
                column: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoCompras_AspNetUsers_IdUsuario",
                table: "CarritoCompras",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoCompras_Producto_IdProducto",
                table: "CarritoCompras",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_AspNetUsers_IdUsuario",
                table: "Pedidos",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categorias_IdCategoria",
                table: "Producto",
                column: "IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
