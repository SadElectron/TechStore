using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    SoldQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PropName = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    File = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropertyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    PropValue = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Details_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "LastUpdate", "Order" },
                values: new object[,]
                {
                    { new Guid("283387f2-0324-4ea3-b0cf-b0a39178ab04"), "Gpu", new DateTime(2024, 8, 30, 14, 20, 50, 493, DateTimeKind.Utc).AddTicks(9481), 2 },
                    { new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), "Cpu", new DateTime(2024, 8, 30, 14, 20, 50, 493, DateTimeKind.Utc).AddTicks(9374), 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "LastUpdate", "Order", "Price", "ProductName", "SoldQuantity", "Stock" },
                values: new object[,]
                {
                    { new Guid("089fd623-946c-46e2-bee6-01771d0d7e57"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1503), 11, 150.0, "AMD Ryzen™ 5 7500F", 0, 50 },
                    { new Guid("0d2bab70-1fd5-402d-8b1a-de7f8d93ce96"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1490), 5, 369.0, "AMD Ryzen™ 9 7900", 0, 30 },
                    { new Guid("1ccfc96a-419e-4940-bb49-8b477455292a"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1501), 10, 180.0, "AMD Ryzen™ 5 7600", 0, 50 },
                    { new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1395), 1, 525.0, "AMD Ryzen™ 9 7950X3D", 0, 30 },
                    { new Guid("291d08a4-4546-4de3-b424-14d97373c207"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1497), 8, 280.0, "AMD Ryzen™ 7 7700", 0, 30 },
                    { new Guid("338ba77b-7b4e-4ea4-988f-9feea94a91d1"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1476), 3, 399.0, "AMD Ryzen™ 9 7900X3D", 0, 30 },
                    { new Guid("91370f1c-19c1-4b3d-9b22-1e812ad211e8"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1499), 9, 195.0, "AMD Ryzen™ 5 7600X", 0, 50 },
                    { new Guid("c547a2f5-764b-4b7d-a557-e819dc618f49"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1492), 6, 365.0, "AMD Ryzen™ 7 7800X3D", 0, 30 },
                    { new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1473), 2, 519.0, "AMD Ryzen™ 9 7950X", 0, 30 },
                    { new Guid("c9b9e9c4-6130-47ca-a5e5-1b23dab32074"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1478), 4, 350.0, "AMD Ryzen™ 9 7900X", 0, 30 },
                    { new Guid("e241fb97-0f53-4534-b4d2-fdf2aadfc380"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(1495), 7, 277.0, "AMD Ryzen™ 7 7700X", 0, 30 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "CategoryId", "LastUpdate", "Order", "PropName" },
                values: new object[,]
                {
                    { new Guid("0c918516-42ed-4e73-b75e-5aa6eb270d2c"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(647), 2, "Family" },
                    { new Guid("11f37990-fed5-41f0-b4e4-cf91f75f645e"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(661), 4, "Core count" },
                    { new Guid("329d6988-517d-4ba0-9c1a-d2dc68b04261"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(666), 7, "Max turbo frequency" },
                    { new Guid("3493f477-64d7-46d0-938d-4c2cf3106dc9"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(663), 5, "Thread count" },
                    { new Guid("374dfd54-383b-49ad-b405-0ac099f3a027"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(678), 9, "Integrated graphics" },
                    { new Guid("5c28a010-3d4a-401a-a980-7c2d762023b9"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(649), 3, "Series" },
                    { new Guid("5f6e3fa8-80ad-4074-a105-c9445ee8c019"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(668), 8, "Socket type" },
                    { new Guid("6461d10f-fe71-449b-bdb2-880675603df8"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(687), 13, "L1 cache" },
                    { new Guid("a1a342d1-45e1-4e56-ad89-b08c021b02e3"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(688), 14, "L2 cache" },
                    { new Guid("a242284f-aaba-4acf-bfb1-55cc556ac85f"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(685), 12, "Launch date" },
                    { new Guid("a8889346-e85e-4f0c-b29e-352d8d1a8666"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(565), 1, "Brand" },
                    { new Guid("d812fa95-4a9d-4a70-9944-1a86174d551b"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(664), 6, "Base clock speed" },
                    { new Guid("e31a2c4b-6e53-4278-9daa-dbf658431f3f"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(682), 11, "Tdp" },
                    { new Guid("f6fbee42-b3c5-44f1-9d63-3c9b3136a060"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(690), 15, "L3 cache" },
                    { new Guid("fb4afebc-c646-4602-9981-3c1f7920d20d"), new Guid("49f07ca0-93dc-44f8-a1ed-46eed86007a8"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(680), 10, "Memory support" }
                });

            migrationBuilder.InsertData(
                table: "Details",
                columns: new[] { "Id", "LastUpdate", "Order", "ProductId", "PropValue", "PropertyId" },
                values: new object[,]
                {
                    { new Guid("0ef1b9fb-cfb7-4c30-9539-89a1f40845f7"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2297), 9, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "AMD Radeon™ Graphics", new Guid("374dfd54-383b-49ad-b405-0ac099f3a027") },
                    { new Guid("172076b4-9cfa-46ef-823f-83038a8985f6"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2284), 3, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "7000", new Guid("5c28a010-3d4a-401a-a980-7c2d762023b9") },
                    { new Guid("1b838db6-d646-40ba-9262-2cd0b4f841ba"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2288), 4, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "16", new Guid("11f37990-fed5-41f0-b4e4-cf91f75f645e") },
                    { new Guid("1fadc053-00e4-4d68-b0a3-18492f24936a"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2295), 8, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "AM5", new Guid("5f6e3fa8-80ad-4074-a105-c9445ee8c019") },
                    { new Guid("25d394cc-f0b3-4b2b-b4eb-5f5187685b5c"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2264), 10, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "DDR5", new Guid("fb4afebc-c646-4602-9981-3c1f7920d20d") },
                    { new Guid("2b1c147b-7b7f-4cd3-bf34-5eb2f3fbb39b"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2250), 4, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "16", new Guid("11f37990-fed5-41f0-b4e4-cf91f75f645e") },
                    { new Guid("2b690feb-8e47-4591-8175-12ff87441580"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2254), 6, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "4.2 GHz", new Guid("d812fa95-4a9d-4a70-9944-1a86174d551b") },
                    { new Guid("3aaf30f6-e5b3-4bb9-8219-8ee950d6e1ad"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2301), 11, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "170W", new Guid("e31a2c4b-6e53-4278-9daa-dbf658431f3f") },
                    { new Guid("47f7ea99-cd27-4935-90f1-b41c3bb787c4"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2290), 5, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "32", new Guid("3493f477-64d7-46d0-938d-4c2cf3106dc9") },
                    { new Guid("4a3f109b-cce9-4c77-aa01-62bd490be62b"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2304), 12, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "2022-09-27", new Guid("a242284f-aaba-4acf-bfb1-55cc556ac85f") },
                    { new Guid("5123f567-43c4-442a-8e9c-3666fcbeaad4"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2293), 7, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "5.7 GHz", new Guid("329d6988-517d-4ba0-9c1a-d2dc68b04261") },
                    { new Guid("5ec0c301-19a8-4eb3-ab5a-4d369441f1c0"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2256), 7, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "5.7 GHz", new Guid("329d6988-517d-4ba0-9c1a-d2dc68b04261") },
                    { new Guid("6e5ebac0-52d4-4269-9cdf-31eb80b2540e"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2299), 10, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "DDR5", new Guid("fb4afebc-c646-4602-9981-3c1f7920d20d") },
                    { new Guid("75ff4238-1429-44a3-8a02-5166f49c0bc0"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2308), 14, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "64 MB", new Guid("f6fbee42-b3c5-44f1-9d63-3c9b3136a060") },
                    { new Guid("7bda6baf-7f28-42fc-b6bd-c614cc4bb543"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2236), 2, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "Ryzen", new Guid("0c918516-42ed-4e73-b75e-5aa6eb270d2c") },
                    { new Guid("89b25603-fd05-436a-8112-33e9fc9b1706"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2261), 9, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "AMD Radeon™ Graphics", new Guid("374dfd54-383b-49ad-b405-0ac099f3a027") },
                    { new Guid("8cc97f2f-e1e8-4b2f-96b5-a36d9a5385fb"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2238), 3, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "7000", new Guid("5c28a010-3d4a-401a-a980-7c2d762023b9") },
                    { new Guid("8f58518e-8141-4d09-ae76-44d61a0fd47b"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2258), 8, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "AM5", new Guid("5f6e3fa8-80ad-4074-a105-c9445ee8c019") },
                    { new Guid("add10f96-5def-4077-b8e4-5e09be32b0d9"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2278), 14, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "128 MB", new Guid("f6fbee42-b3c5-44f1-9d63-3c9b3136a060") },
                    { new Guid("b83d3ff2-abac-472c-a55c-6ad3595a2795"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2306), 13, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "16 MB", new Guid("a1a342d1-45e1-4e56-ad89-b08c021b02e3") },
                    { new Guid("c0b4dd42-295a-45ea-a45a-8e253d962c9e"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2280), 1, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "AMD", new Guid("a8889346-e85e-4f0c-b29e-352d8d1a8666") },
                    { new Guid("caff9ff0-9b43-4818-8c07-102b6695a55c"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2291), 6, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "4.5 GHz", new Guid("d812fa95-4a9d-4a70-9944-1a86174d551b") },
                    { new Guid("d95a055a-e201-4fbb-b478-a914818dc687"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2252), 5, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "32", new Guid("3493f477-64d7-46d0-938d-4c2cf3106dc9") },
                    { new Guid("e511ec95-de70-42b1-aba1-74bd8d973483"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2266), 11, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "120W", new Guid("e31a2c4b-6e53-4278-9daa-dbf658431f3f") },
                    { new Guid("f27c0a8f-fd35-4654-a298-a1036bf0b4c3"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2282), 2, new Guid("c861a5e8-187b-4a55-ad62-53f4f7574360"), "Ryzen", new Guid("0c918516-42ed-4e73-b75e-5aa6eb270d2c") },
                    { new Guid("f4955233-181d-4736-9034-b5f7d1288ca3"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2160), 1, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "AMD", new Guid("a8889346-e85e-4f0c-b29e-352d8d1a8666") },
                    { new Guid("f8cbda97-bd7d-4d90-b100-1930eb3fedec"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2270), 13, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "16 MB", new Guid("a1a342d1-45e1-4e56-ad89-b08c021b02e3") },
                    { new Guid("fa79c502-d800-48ce-a06a-0ce3191c8ed1"), new DateTime(2024, 8, 30, 14, 20, 50, 494, DateTimeKind.Utc).AddTicks(2268), 12, new Guid("216ac5c2-f5ea-4a74-9fe0-889a9d7a6735"), "2023-02-28", new Guid("a242284f-aaba-4acf-bfb1-55cc556ac85f") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Details_ProductId",
                table: "Details",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_PropertyId",
                table: "Details",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CategoryId",
                table: "Properties",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
