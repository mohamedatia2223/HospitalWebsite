using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class FixedNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Years_of_Experience",
                table: "Doctors",
                newName: "YearsOfExperience");

            migrationBuilder.RenameColumn(
                name: "Contact_Info",
                table: "Doctors",
                newName: "ContactInfo");

            migrationBuilder.RenameColumn(
                name: "Number_Of_Staff",
                table: "Departments",
                newName: "NumberOfStaff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearsOfExperience",
                table: "Doctors",
                newName: "Years_of_Experience");

            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "Doctors",
                newName: "Contact_Info");

            migrationBuilder.RenameColumn(
                name: "NumberOfStaff",
                table: "Departments",
                newName: "Number_Of_Staff");
        }
    }
}
