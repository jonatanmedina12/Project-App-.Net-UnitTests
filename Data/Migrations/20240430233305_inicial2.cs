using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class inicial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Fecha_Nacimiento",
                table: "pacientes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_FechaNacimiento",
                table: "pacientes",
                column: "Fecha_Nacimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_Nombre",
                table: "pacientes",
                column: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paciente_FechaNacimiento",
                table: "pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_Nombre",
                table: "pacientes");

            migrationBuilder.AlterColumn<string>(
                name: "Fecha_Nacimiento",
                table: "pacientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
