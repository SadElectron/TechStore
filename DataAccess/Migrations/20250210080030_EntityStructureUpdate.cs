using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EntityStructureUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Categories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "RowOrder",
                table: "UserData",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserData",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "UserData",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "RefreshTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "RefreshTokens",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Properties",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Properties",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Products",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "RowOrder",
                table: "OrderProduct",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderProduct",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "OrderProduct",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "RowOrder",
                table: "Order",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Order",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Images",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Images",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Details",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Details",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "RowOrder",
                table: "Categories",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RowOrder",
                table: "UserData",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Properties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RowOrder",
                table: "OrderProduct",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "RowOrder",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Details",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
