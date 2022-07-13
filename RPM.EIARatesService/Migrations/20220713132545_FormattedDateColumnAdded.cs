using Microsoft.EntityFrameworkCore.Migrations;

namespace RPM.EIARatesService.Migrations
{
    public partial class FormattedDateColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormattedDate",
                table: "Rates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormattedDate",
                table: "Rates");
        }
    }
}
