using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeManagement.Migrations
{
    /// <inheritdoc />
    public partial class MakeStudentFManagerFidNullableAndSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fManager_faculty_fid",
                table: "fManager");

            migrationBuilder.DropForeignKey(
                name: "FK_student_faculty_fid",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "fid",
                table: "student",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "fid",
                table: "fManager",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_fManager_faculty_fid",
                table: "fManager",
                column: "fid",
                principalTable: "faculty",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_student_faculty_fid",
                table: "student",
                column: "fid",
                principalTable: "faculty",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fManager_faculty_fid",
                table: "fManager");

            migrationBuilder.DropForeignKey(
                name: "FK_student_faculty_fid",
                table: "student");

            migrationBuilder.UpdateData(
                table: "student",
                keyColumn: "fid",
                keyValue: null,
                column: "fid",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "fid",
                table: "student",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "fManager",
                keyColumn: "fid",
                keyValue: null,
                column: "fid",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "fid",
                table: "fManager",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_fManager_faculty_fid",
                table: "fManager",
                column: "fid",
                principalTable: "faculty",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_faculty_fid",
                table: "student",
                column: "fid",
                principalTable: "faculty",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
