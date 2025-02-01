using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeBeanAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    isBOTD = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<string>(type: "nvarchar(max)", precision: 18, scale: 2, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeanOfTheDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeanOfTheDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeanOfTheDays_Beans_BeanId",
                        column: x => x.BeanId,
                        principalTable: "Beans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeanOfTheDays_BeanId",
                table: "BeanOfTheDays",
                column: "BeanId");

            migrationBuilder.CreateIndex(
                name: "IX_BeanOfTheDays_SelectedDate",
                table: "BeanOfTheDays",
                column: "SelectedDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeanOfTheDays");

            migrationBuilder.DropTable(
                name: "Beans");
        }
    }
}
