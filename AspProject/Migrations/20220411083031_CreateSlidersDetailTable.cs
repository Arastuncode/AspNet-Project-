using Microsoft.EntityFrameworkCore.Migrations;

namespace AspProject.Migrations
{
    public partial class CreateSlidersDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SliderDetails_Sliders_SliderId",
                table: "SliderDetails");

            migrationBuilder.DropIndex(
                name: "IX_SliderDetails_SliderId",
                table: "SliderDetails");

            migrationBuilder.DropColumn(
                name: "SliderId",
                table: "SliderDetails");

            migrationBuilder.AddColumn<int>(
                name: "SliderDetailId",
                table: "Sliders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_SliderDetailId",
                table: "Sliders",
                column: "SliderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_SliderDetails_SliderDetailId",
                table: "Sliders",
                column: "SliderDetailId",
                principalTable: "SliderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_SliderDetails_SliderDetailId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_SliderDetailId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "SliderDetailId",
                table: "Sliders");

            migrationBuilder.AddColumn<int>(
                name: "SliderId",
                table: "SliderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SliderDetails_SliderId",
                table: "SliderDetails",
                column: "SliderId");

            migrationBuilder.AddForeignKey(
                name: "FK_SliderDetails_Sliders_SliderId",
                table: "SliderDetails",
                column: "SliderId",
                principalTable: "Sliders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
