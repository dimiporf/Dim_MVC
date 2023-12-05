using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DimWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CompanyTableFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Companies",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "ID");
        }
    }
}
