using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test.Migrations
{
    /// <inheritdoc />
    public partial class updateCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إسقاط العمود القديم
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartItems");

            // إضافة العمود الجديد مع خصائص IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CartItems",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إسقاط العمود الجديد
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartItems");

            // إعادة العمود القديم (إذا كان هناك خصائص سابقة)
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1"); // أو خصائص سابقة إذا كانت موجودة
        }
    }
}
