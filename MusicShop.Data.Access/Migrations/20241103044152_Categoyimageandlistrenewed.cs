using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicShop.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class Categoyimageandlistrenewed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/image/Per.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/image/key.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/image/win.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/pa.jpg", "PA Systems" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/rec.jpg", "Recording " });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/el.jpg", "Electric" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/acc.jpg", "Accessories " });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/dj.jpg", "DJ Equipments" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/image/Per.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/image/key.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/image/win.jpeg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/folk.jpeg", "Folk & Ethnic Instruments" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/rec.jpeg", "Recording & Studio Gear" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/pro.jpeg", "Pro Audio Equipment" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/acc.jpeg", "Accessories & Gear" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "/image/band.jpeg", "Bundles & Deals" });
        }
    }
}
