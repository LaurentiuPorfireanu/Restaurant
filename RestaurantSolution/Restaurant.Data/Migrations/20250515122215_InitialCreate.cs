using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alergen",
                columns: table => new
                {
                    AlergenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergen", x => x.AlergenID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuID);
                    table.ForeignKey(
                        name: "FK_Menu_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Preparat",
                columns: table => new
                {
                    PreparatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    QuantityPortie = table.Column<int>(type: "int", nullable: false),
                    QuantityTotal = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preparat", x => x.PreparatID);
                    table.ForeignKey(
                        name: "FK_Preparat_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EstimatedDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuPreparat",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    PreparatID = table.Column<int>(type: "int", nullable: false),
                    QuantityMenuPortie = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPreparat", x => new { x.MenuID, x.PreparatID });
                    table.ForeignKey(
                        name: "FK_MenuPreparat_Menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menu",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPreparat_Preparat_PreparatID",
                        column: x => x.PreparatID,
                        principalTable: "Preparat",
                        principalColumn: "PreparatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparatAlergen",
                columns: table => new
                {
                    PreparatID = table.Column<int>(type: "int", nullable: false),
                    AlergenID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparatAlergen", x => new { x.PreparatID, x.AlergenID });
                    table.ForeignKey(
                        name: "FK_PreparatAlergen_Alergen_AlergenID",
                        column: x => x.AlergenID,
                        principalTable: "Alergen",
                        principalColumn: "AlergenID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreparatAlergen_Preparat_PreparatID",
                        column: x => x.PreparatID,
                        principalTable: "Preparat",
                        principalColumn: "PreparatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparatFoto",
                columns: table => new
                {
                    FotoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PreparatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparatFoto", x => x.FotoID);
                    table.ForeignKey(
                        name: "FK_PreparatFoto_Preparat_PreparatID",
                        column: x => x.PreparatID,
                        principalTable: "Preparat",
                        principalColumn: "PreparatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDish",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    PreparatID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDish", x => new { x.OrderID, x.PreparatID });
                    table.ForeignKey(
                        name: "FK_OrderDish_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDish_Preparat_PreparatID",
                        column: x => x.PreparatID,
                        principalTable: "Preparat",
                        principalColumn: "PreparatID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderMenu",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMenu", x => new { x.OrderID, x.MenuID });
                    table.ForeignKey(
                        name: "FK_OrderMenu_Menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menu",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMenu_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alergen_Name",
                table: "Alergen",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CategoryID",
                table: "Menu",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPreparat_PreparatID",
                table: "MenuPreparat",
                column: "PreparatID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderCode",
                table: "Order",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDish_PreparatID",
                table: "OrderDish",
                column: "PreparatID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMenu_MenuID",
                table: "OrderMenu",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Preparat_CategoryID",
                table: "Preparat",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PreparatAlergen_AlergenID",
                table: "PreparatAlergen",
                column: "AlergenID");

            migrationBuilder.CreateIndex(
                name: "IX_PreparatFoto_PreparatID",
                table: "PreparatFoto",
                column: "PreparatID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuPreparat");

            migrationBuilder.DropTable(
                name: "OrderDish");

            migrationBuilder.DropTable(
                name: "OrderMenu");

            migrationBuilder.DropTable(
                name: "PreparatAlergen");

            migrationBuilder.DropTable(
                name: "PreparatFoto");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Alergen");

            migrationBuilder.DropTable(
                name: "Preparat");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
