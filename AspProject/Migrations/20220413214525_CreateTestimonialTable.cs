using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspProject.Migrations
{
    public partial class CreateTestimonialTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Speakers_SpeakerIdId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Eventss");

            migrationBuilder.DropIndex(
                name: "IX_Events_SpeakerIdId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SpeakerIdId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Imge = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.AddColumn<int>(
                name: "SpeakerIdId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Eventss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventss", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SpeakerIdId",
                table: "Events",
                column: "SpeakerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Speakers_SpeakerIdId",
                table: "Events",
                column: "SpeakerIdId",
                principalTable: "Speakers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
