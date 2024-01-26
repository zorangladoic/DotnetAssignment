using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowrSpot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTablesSightingsLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sightings",
                columns: new[] { "Id", "FlowerId", "Latitude", "Longitude", "UserId" },
                values: new object[] { new Guid("be561c74-4282-49af-94a4-5d3e2e146276"), new Guid("a9537e9e-b3df-4f91-9eb3-f6bf27026ff0"), "678,332", "467.345", new Guid("7a7782d7-231f-4239-a489-aa0fc53f7012") });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "SightingId", "UserId" },
                values: new object[] { new Guid("be561c74-4282-49af-94a4-5d3e2e146276"), new Guid("aa39a862-764b-4797-938f-e562150393f9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Likes",
                keyColumns: new[] { "SightingId", "UserId" },
                keyValues: new object[] { new Guid("be561c74-4282-49af-94a4-5d3e2e146276"), new Guid("aa39a862-764b-4797-938f-e562150393f9") });

            migrationBuilder.DeleteData(
                table: "Sightings",
                keyColumn: "Id",
                keyValue: new Guid("be561c74-4282-49af-94a4-5d3e2e146276"));
        }
    }
}
