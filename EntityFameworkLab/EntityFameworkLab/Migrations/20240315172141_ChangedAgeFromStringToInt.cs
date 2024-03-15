using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFameworkLab.Migrations
{
    public partial class ChangedAgeFromStringToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            "UPDATE People SET Age = CASE " +
            "WHEN Age = 'Forty' THEN 40 " +
            "END WHERE Age IN('Forty')");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "People",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Add a similar SQL Query at the end of the Down method to turn the integer 40 into the string ‘Forty’ if we want to roll back this migration.
            migrationBuilder.Sql(
                "UPDATE People SET Age = CASE " +
                "WHEN Age = 40 THEN 'Forty' " +
                "END WHERE Age IN(40)"
                );

                migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
