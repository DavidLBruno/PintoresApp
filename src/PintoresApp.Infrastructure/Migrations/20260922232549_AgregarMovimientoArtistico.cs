using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PintoresApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMovimientoArtistico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovimientoArtisticoId",
                table: "Pintores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovimientosArtisticos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    PaisOrigen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosArtisticos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pintores_MovimientoArtisticoId",
                table: "Pintores",
                column: "MovimientoArtisticoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pintores_MovimientosArtisticos_MovimientoArtisticoId",
                table: "Pintores",
                column: "MovimientoArtisticoId",
                principalTable: "MovimientosArtisticos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pintores_MovimientosArtisticos_MovimientoArtisticoId",
                table: "Pintores");

            migrationBuilder.DropTable(
                name: "MovimientosArtisticos");

            migrationBuilder.DropIndex(
                name: "IX_Pintores_MovimientoArtisticoId",
                table: "Pintores");

            migrationBuilder.DropColumn(
                name: "MovimientoArtisticoId",
                table: "Pintores");
        }
    }
}
