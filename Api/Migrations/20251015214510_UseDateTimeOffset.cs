using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UseDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Contenido", "Creada", "Titulo" },
                values: new object[,]
                {
                    { 1, "Configuración inicial del proyecto .NET con Docker y Postgres.", new DateTimeOffset(new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Primer apunte" },
                    { 2, "Agregar validaciones de modelo y respuestas 400/404 en los endpoints.", new DateTimeOffset(new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Recordatorio" },
                    { 3, "Probar el deploy en Render y verificar las migraciones automáticas.", new DateTimeOffset(new DateTime(2024, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Pendiente" },
                    { 4, "Implementar JWT para autenticar las operaciones sobre notas.", new DateTimeOffset(new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Idea futura" },
                    { 5, "Recordar CAMS: Culture, Automation, Measurement, Sharing.", new DateTimeOffset(new DateTime(2024, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machete DevOps" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
