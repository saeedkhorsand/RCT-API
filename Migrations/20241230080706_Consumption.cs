using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspnetCoreMvcFull.Migrations
{
  /// <inheritdoc />
  public partial class Consumption : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Product",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            GroupId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Product", x => x.Id);
            table.ForeignKey(
          name: "FK_Product_Groups_GroupId",
          column: x => x.GroupId,
          principalTable: "Groups",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade); // حذف آبشاری فعال برای Group
          });

      migrationBuilder.CreateTable(
          name: "Consumption",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            UserId = table.Column<int>(type: "int", nullable: false),
            UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
            ProductId = table.Column<int>(type: "int", nullable: false),
            ConsumptionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            Quantity = table.Column<double>(type: "float", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Consumption", x => x.Id);
            table.ForeignKey(
          name: "FK_Consumption_AspNetUsers_UserId1",
          column: x => x.UserId1,
          principalTable: "AspNetUsers",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict); // حذف آبشاری غیرفعال برای User
            table.ForeignKey(
          name: "FK_Consumption_Product_ProductId",
          column: x => x.ProductId,
          principalTable: "Product",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade); // حذف آبشاری فعال برای Product
          });

      migrationBuilder.CreateIndex(
          name: "IX_Consumption_ProductId",
          table: "Consumption",
          column: "ProductId");

      migrationBuilder.CreateIndex(
          name: "IX_Consumption_UserId1",
          table: "Consumption",
          column: "UserId1");

      migrationBuilder.CreateIndex(
          name: "IX_Product_GroupId",
          table: "Product",
          column: "GroupId");

    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Consumption");

      migrationBuilder.DropTable(
          name: "Product");
    }
  }
}
