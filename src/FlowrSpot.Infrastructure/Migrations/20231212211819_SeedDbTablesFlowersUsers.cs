using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowrSpot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDbTablesFlowersUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Flowers",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("60164c52-e90c-4315-9f4e-95ae1c9f1f03"), "Red rose", "some/imge/rose_url.jpg", "Rose" },
                    { new Guid("749778b3-6756-45cc-a5fb-64e8162f5ce8"), "Yellow tulip", "some/imge/tulip_url.jpg", "Tulip" },
                    { new Guid("a9537e9e-b3df-4f91-9eb3-f6bf27026ff0"), "Yellow daffodil", "some/imge/daffodil_url.jpg", "Daffodil" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("7a7782d7-231f-4239-a489-aa0fc53f7012"), "test123@yahoo.com", "test123", "test123" },
                    { new Guid("aa39a862-764b-4797-938f-e562150393f9"), "bob44@yahoo.com", "bob123", "Bob44" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: new Guid("60164c52-e90c-4315-9f4e-95ae1c9f1f03"));

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: new Guid("749778b3-6756-45cc-a5fb-64e8162f5ce8"));

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: new Guid("a9537e9e-b3df-4f91-9eb3-f6bf27026ff0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7a7782d7-231f-4239-a489-aa0fc53f7012"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aa39a862-764b-4797-938f-e562150393f9"));
        }
    }
}
