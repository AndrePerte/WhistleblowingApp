using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhistleblowingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Segnalazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codice = table.Column<string>(type: "TEXT", nullable: false),
                    Descrizione = table.Column<string>(type: "TEXT", nullable: false),
                    PersoneCoinvolte = table.Column<string>(type: "TEXT", nullable: true),
                    AltriAConoscenza = table.Column<string>(type: "TEXT", nullable: true),
                    DataIllecito = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OraIllecito = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    RischioRitorsione = table.Column<bool>(type: "INTEGER", nullable: false),
                    PericoloImminente = table.Column<bool>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Cognome = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telefono = table.Column<string>(type: "TEXT", nullable: true),
                    Stato = table.Column<string>(type: "TEXT", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segnalazioni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessaggi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Testo = table.Column<string>(type: "TEXT", nullable: false),
                    Mittente = table.Column<string>(type: "TEXT", nullable: false),
                    DataInvio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SegnalazioneId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessaggi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessaggi_Segnalazioni_SegnalazioneId",
                        column: x => x.SegnalazioneId,
                        principalTable: "Segnalazioni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessaggi_SegnalazioneId",
                table: "ChatMessaggi",
                column: "SegnalazioneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessaggi");

            migrationBuilder.DropTable(
                name: "Segnalazioni");
        }
    }
}
