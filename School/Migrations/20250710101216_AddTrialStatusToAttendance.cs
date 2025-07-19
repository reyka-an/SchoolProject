using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class AddTrialStatusToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrial",
                table: "Attendances");

            migrationBuilder.InsertData(
                table: "AttendanceStatuses",
                columns: new[] { "Id", "Code", "Description", "Label" },
                values: new object[] { 5, "trial", null, "Пробное" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AttendanceStatuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrial",
                table: "Attendances",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
