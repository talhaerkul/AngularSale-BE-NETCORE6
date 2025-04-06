using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularSaleAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefinitionType",
                table: "Endpoints",
                newName: "Definition");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Definition",
                table: "Endpoints",
                newName: "DefinitionType");
        }
    }
}
