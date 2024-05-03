using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CPUs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    CoreCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ThreadCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseClockSpeed = table.Column<string>(type: "TEXT", nullable: false),
                    MaxTurboFrequency = table.Column<string>(type: "TEXT", nullable: false),
                    CacheSize = table.Column<string>(type: "TEXT", nullable: false),
                    SocketType = table.Column<string>(type: "TEXT", nullable: false),
                    IntegratedGraphics = table.Column<string>(type: "TEXT", nullable: false),
                    MemorySupport = table.Column<string>(type: "TEXT", nullable: false),
                    Tdp = table.Column<string>(type: "TEXT", nullable: false),
                    LaunchDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GPUs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    GraphicEngine = table.Column<string>(type: "TEXT", nullable: false),
                    VramSize = table.Column<string>(type: "TEXT", nullable: false),
                    VramType = table.Column<string>(type: "TEXT", nullable: false),
                    BaseClockSpeed = table.Column<string>(type: "TEXT", nullable: false),
                    BoostClockSpeed = table.Column<string>(type: "TEXT", nullable: false),
                    MemoryBusWidth = table.Column<string>(type: "TEXT", nullable: false),
                    Tdp = table.Column<string>(type: "TEXT", nullable: false),
                    PowerConnectors = table.Column<string>(type: "TEXT", nullable: false),
                    Interface = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayPorts = table.Column<string>(type: "TEXT", nullable: false),
                    LaunchDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GPUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<bool>(type: "INTEGER", nullable: false),
                    File = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_CPUs_ProductId",
                        column: x => x.ProductId,
                        principalTable: "CPUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_GPUs_ProductId",
                        column: x => x.ProductId,
                        principalTable: "GPUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CPUs",
                columns: new[] { "Id", "BaseClockSpeed", "Brand", "CacheSize", "CoreCount", "IntegratedGraphics", "LaunchDate", "MaxTurboFrequency", "MemorySupport", "ModelName", "Price", "SocketType", "Tdp", "ThreadCount" },
                values: new object[,]
                {
                    { new Guid("080e60f6-7c67-423f-9396-726f5166384d"), "3.7 GHz", "AMD", "64 MB", 12, "None", new DateOnly(2020, 11, 19), "4.8 GHz", "DDR4-3200", "Ryzen 9 5900X", 549.99000000000001, "AM4", "105W", 24 },
                    { new Guid("08a0c8af-7396-4394-a32b-0ed201760864"), "3.6 GHz", "AMD", "19 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.4 GHz", "DDR4-3200", "Ryzen 3 5500", 159.99000000000001, "AM4", "65W", 12 },
                    { new Guid("2716d97e-d59c-4342-916d-a930278b79eb"), "4.0 GHz", "AMD", "40 MB", 4, "AMD Radeon Graphics Vega 6", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 3 5300G", 149.99000000000001, "AM4", "65W", 8 },
                    { new Guid("4e286288-9e84-40aa-b01b-9f7d6b8c859f"), "3.6 GHz", "Intel", "25 MB", 8, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "5.2 GHz", "DDR4-3200", "Core i7-12700K", 439.99000000000001, "LGA 1700", "125W", 16 },
                    { new Guid("50a85ed7-2761-441d-9fc0-93aecdd1ffca"), "2.5 GHz", "Intel", "18 MB", 6, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400", 229.99000000000001, "LGA 1700", "65W", 12 },
                    { new Guid("861c47c1-b755-4dbd-b4d2-bf5b2c23194f"), "3.8 GHz", "AMD", "36 MB", 8, "AMD Radeon Graphics Vega 8", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 7 5700G", 349.99000000000001, "AM4", "65W", 16 },
                    { new Guid("a7722d03-3619-4f92-94a6-afd43dc430da"), "2.5 GHz", "Intel", "18 MB", 6, "None", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400F", 199.99000000000001, "LGA 1700", "65W", 12 },
                    { new Guid("dba4c197-72f2-4c2a-a751-912919fd7811"), "3.3 GHz", "Intel", "12 MB", 4, "Intel UHD Graphics 730", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100", 129.99000000000001, "LGA 1700", "60W", 8 },
                    { new Guid("e06f3824-7e4d-4b54-b96d-b772ad0b6e6a"), "3.5 GHz", "Intel", "20 MB", 8, "Intel UHD Graphics 770", new DateOnly(2021, 3, 18), "5.3 GHz", "DDR4-3200", "Core i9-11900K", 499.99000000000001, "LGA 1200", "125W", 16 },
                    { new Guid("ec4512e8-ccce-48b5-86b1-1888d93c5cee"), "3.7 GHz", "AMD", "38 MB", 6, "None", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 5 5600X", 299.99000000000001, "AM4", "65W", 12 },
                    { new Guid("ec8d2113-75a7-4694-bd7e-1e6cefb4ba66"), "3.5 GHz", "AMD", "32 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.5 GHz", "DDR4-3200", "Ryzen 5 7600", 229.99000000000001, "AM4", "65W", 12 },
                    { new Guid("fe96a5bd-f62a-4b0c-84f9-939af5c828c2"), "3.3 GHz", "Intel", "12 MB", 4, "None", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100F", 109.98999999999999, "LGA 1700", "60W", 8 }
                });

            migrationBuilder.InsertData(
                table: "GPUs",
                columns: new[] { "Id", "BaseClockSpeed", "BoostClockSpeed", "Brand", "DisplayPorts", "GraphicEngine", "Interface", "LaunchDate", "MemoryBusWidth", "ModelName", "PowerConnectors", "Price", "Tdp", "VramSize", "VramType" },
                values: new object[,]
                {
                    { new Guid("0af79d02-9c43-40e7-ac5e-cc8b278d899e"), "2530 MHz", "2575 MHz", "ASUS", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 4070 Ti", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "TUF Gaming", "8-pin x 2", 799.99000000000001, "285W", "10GB", "GDDR6X" },
                    { new Guid("2a9622d9-dc98-40fa-84d4-396e947267e1"), "1720 MHz", "1807 MHz", "PNY", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3050", "PCIe 4.0 x16", new DateOnly(2021, 1, 27), "128 Bit", "XLR8 RTX 3050", "6-pin x 1", 279.99000000000001, "130W", "8GB", "GDDR6" },
                    { new Guid("40e077e2-b9a3-4b4c-82ad-d0fcc946ea9d"), "2420 MHz", "2680 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "Radeon RX 6700 XT", "PCIe 4.0 x16", new DateOnly(2022, 3, 18), "256 Bit", "Radeon RX 6700 XT MECH 2X", "8-pin x 1", 459.99000000000001, "230W", "12GB", "GDDR6" },
                    { new Guid("53ae14f0-9ff6-471d-a1d0-4fcd1e9fb3ca"), "2580 MHz", "2670 MHz", "XFX", "DisplayPort x 2 (v2.0) HDMI™ x 1", "Radeon RX 6650 XT", "PCIe 4.0 x16", new DateOnly(2023, 5, 10), "128 Bit", "Speedster MERC 310", "8-pin x 1", 349.99000000000001, "180W", "8GB", "GDDR6" },
                    { new Guid("67734159-2235-4179-8bbc-fe36c8dbed95"), "1580 MHz", "1830 MHz", "Inno3D", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3070 Ti", "PCIe 4.0 x16", new DateOnly(2021, 6, 1), "256 Bit", "Twin X2 OC", "8-pin x 2", 499.99000000000001, "290W", "8GB", "GDDR6X" },
                    { new Guid("7d20cbad-c22f-4847-9e0a-83226d7dddcd"), "1410 MHz", "1700 MHz", "EVGA", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3060 Ti", "PCIe 4.0 x16", new DateOnly(2021, 11, 17), "256 Bit", "GeForce RTX 3060 Ti", "8-pin x 1", 399.99000000000001, "200W", "8GB", "GDDR6" },
                    { new Guid("89824f68-8f43-4a35-91c3-681740046713"), "2300 MHz", "2700 MHz", "PowerColor", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7800 XT", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "Red Devil", "8-pin x 3", 849.99000000000001, "330W", "16GB", "GDDR6" },
                    { new Guid("97a7bd3a-647e-4a47-b27b-85104ec67dfc"), "2400 MHz", "2600 MHz", "Gigabyte", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7900 XTX", "PCIe 5.0 x16", new DateOnly(2023, 12, 15), "384 Bit", "AORUS MASTER", "8-pin x 3", 1399.99, "350W", "24GB", "GDDR6" },
                    { new Guid("a1beed27-1428-42e3-843d-290fcc9e516b"), "2100 MHz", "2045 MHz", "ASRock", "DisplayPort x 2 (v1.4) HDMI™ x 2", "Radeon RX 6600", "PCIe 4.0 x16", new DateOnly(2021, 8, 10), "128 Bit", "Challenger D", "6-pin x 1", 229.99000000000001, "130W", "8GB", "GDDR6" },
                    { new Guid("e7daf64c-07c3-4707-b34b-ef4726d0450c"), "2475 MHz", "2490 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX™ 4070", "PCIe 5.0 x16", new DateOnly(2023, 11, 15), "192 Bit", "VENTUS", "8-pin + 6-pin", 499.99000000000001, "200W", "12GB", "GDDR6X" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "CPUs");

            migrationBuilder.DropTable(
                name: "GPUs");
        }
    }
}
