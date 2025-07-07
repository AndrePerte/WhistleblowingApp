using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhistleblowingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAllegati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allegati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeFile = table.Column<string>(type: "TEXT", nullable: false),
                    TipoMime = table.Column<string>(type: "TEXT", nullable: false),
                    Dimensione = table.Column<long>(type: "INTEGER", nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SegnalazioneId = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allegati", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allegati_Segnalazioni_SegnalazioneId",
                        column: x => x.SegnalazioneId,
                        principalTable: "Segnalazioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allegati_SegnalazioneId",
                table: "Allegati",
                column: "SegnalazioneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allegati");
        }
    }
}
