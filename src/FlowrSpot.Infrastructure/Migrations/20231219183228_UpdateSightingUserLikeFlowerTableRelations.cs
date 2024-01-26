using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowrSpot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSightingUserLikeFlowerTableRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sightings_FlowerId",
                table: "Sightings");

            migrationBuilder.CreateIndex(
                name: "IX_Sightings_FlowerId",
                table: "Sightings",
                column: "FlowerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sightings_FlowerId",
                table: "Sightings");

            migrationBuilder.CreateIndex(
                name: "IX_Sightings_FlowerId",
                table: "Sightings",
                column: "FlowerId");
        }
    }
}
