using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspnetCoreMvcFull.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Groups_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumption_AspNetUsers_UserId1",
                table: "Consumption");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumption_Product_ProductId",
                table: "Consumption");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Groups_GroupId",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumption",
                table: "Consumption");

            migrationBuilder.DropIndex(
                name: "IX_Consumption_UserId1",
                table: "Consumption");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Consumption");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Consumption",
                newName: "Consumptions");

            migrationBuilder.RenameIndex(
                name: "IX_Product_GroupId",
                table: "Products",
                newName: "IX_Products_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Consumption_ProductId",
                table: "Consumptions",
                newName: "IX_Consumptions_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Consumptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Consumptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumptions",
                table: "Consumptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Consumptions_ApplicationUserId",
                table: "Consumptions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumptions_UserId",
                table: "Consumptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Groups_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_AspNetUsers_ApplicationUserId",
                table: "Consumptions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_AspNetUsers_UserId",
                table: "Consumptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_Products_ProductId",
                table: "Consumptions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Groups_GroupId",
                table: "Products",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Groups_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_AspNetUsers_ApplicationUserId",
                table: "Consumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_AspNetUsers_UserId",
                table: "Consumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_Products_ProductId",
                table: "Consumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Groups_GroupId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumptions",
                table: "Consumptions");

            migrationBuilder.DropIndex(
                name: "IX_Consumptions_ApplicationUserId",
                table: "Consumptions");

            migrationBuilder.DropIndex(
                name: "IX_Consumptions_UserId",
                table: "Consumptions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Consumptions");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Consumptions",
                newName: "Consumption");

            migrationBuilder.RenameIndex(
                name: "IX_Products_GroupId",
                table: "Product",
                newName: "IX_Product_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Consumptions_ProductId",
                table: "Consumption",
                newName: "IX_Consumption_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Consumption",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Consumption",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumption",
                table: "Consumption",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Consumption_UserId1",
                table: "Consumption",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Groups_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consumption_AspNetUsers_UserId1",
                table: "Consumption",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consumption_Product_ProductId",
                table: "Consumption",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Groups_GroupId",
                table: "Product",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
