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
                });

            migrationBuilder.InsertData(
                table: "CPUs",
                columns: new[] { "Id", "BaseClockSpeed", "Brand", "CacheSize", "CoreCount", "IntegratedGraphics", "LaunchDate", "MaxTurboFrequency", "MemorySupport", "ModelName", "Price", "SocketType", "Tdp", "ThreadCount" },
                values: new object[,]
                {
                    { new Guid("000fb8a0-bb82-430b-a701-9704d52d62cd"), "3.5 GHz", "AMD", "32 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.5 GHz", "DDR4-3200", "Ryzen 5 7600", 229.99000000000001, "AM4", "65W", 12 },
                    { new Guid("10722dda-ee63-4bdb-9e60-4e4585ce623e"), "3.3 GHz", "Intel", "12 MB", 4, "None", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100F", 109.98999999999999, "LGA 1700", "60W", 8 },
                    { new Guid("13cab3be-62ca-4b99-9440-d0748611876d"), "3.6 GHz", "Intel", "25 MB", 8, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "5.2 GHz", "DDR4-3200", "Core i7-12700K", 439.99000000000001, "LGA 1700", "125W", 16 },
                    { new Guid("1ca9f9a0-cb57-40ed-ab19-31f0bf0a9647"), "3.7 GHz", "AMD", "38 MB", 6, "None", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 5 5600X", 299.99000000000001, "AM4", "65W", 12 },
                    { new Guid("27eac8e5-59bf-41d5-830b-6a5b5131b5b4"), "3.3 GHz", "Intel", "12 MB", 4, "Intel UHD Graphics 730", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100", 129.99000000000001, "LGA 1700", "60W", 8 },
                    { new Guid("44c4e17f-3f9c-433c-b9b5-3b7fbf771154"), "4.0 GHz", "AMD", "40 MB", 4, "AMD Radeon Graphics Vega 6", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 3 5300G", 149.99000000000001, "AM4", "65W", 8 },
                    { new Guid("4bbd9ac1-2d10-4746-b66c-8973191466fc"), "3.7 GHz", "AMD", "64 MB", 12, "None", new DateOnly(2020, 11, 19), "4.8 GHz", "DDR4-3200", "Ryzen 9 5900X", 549.99000000000001, "AM4", "105W", 24 },
                    { new Guid("657602c4-5a87-4431-ad77-3eb17e2dbc7a"), "3.8 GHz", "AMD", "36 MB", 8, "AMD Radeon Graphics Vega 8", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 7 5700G", 349.99000000000001, "AM4", "65W", 16 },
                    { new Guid("824663d5-8b1c-4416-9843-c1c2af263e0e"), "2.5 GHz", "Intel", "18 MB", 6, "None", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400F", 199.99000000000001, "LGA 1700", "65W", 12 },
                    { new Guid("85e953ce-eae2-47e9-9481-28fb9b36d7d7"), "3.6 GHz", "AMD", "19 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.4 GHz", "DDR4-3200", "Ryzen 3 5500", 159.99000000000001, "AM4", "65W", 12 },
                    { new Guid("a821c299-0749-4078-be86-e8522132fba7"), "3.5 GHz", "Intel", "20 MB", 8, "Intel UHD Graphics 770", new DateOnly(2021, 3, 18), "5.3 GHz", "DDR4-3200", "Core i9-11900K", 499.99000000000001, "LGA 1200", "125W", 16 },
                    { new Guid("d00b83c3-6ef2-422f-88e8-883b95dfd0c0"), "2.5 GHz", "Intel", "18 MB", 6, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400", 229.99000000000001, "LGA 1700", "65W", 12 }
                });

            migrationBuilder.InsertData(
                table: "GPUs",
                columns: new[] { "Id", "BaseClockSpeed", "BoostClockSpeed", "Brand", "DisplayPorts", "GraphicEngine", "Interface", "LaunchDate", "MemoryBusWidth", "ModelName", "PowerConnectors", "Price", "Tdp", "VramSize", "VramType" },
                values: new object[,]
                {
                    { new Guid("04054bab-7e06-4bd4-80e9-6b7fea7260a8"), "2100 MHz", "2045 MHz", "ASRock", "DisplayPort x 2 (v1.4) HDMI™ x 2", "Radeon RX 6600", "PCIe 4.0 x16", new DateOnly(2021, 8, 10), "128 Bit", "Challenger D", "6-pin x 1", 229.99000000000001, "130W", "8GB", "GDDR6" },
                    { new Guid("0e92b88a-d90d-43d2-9bf9-f06093a34e4c"), "2300 MHz", "2700 MHz", "PowerColor", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7800 XT", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "Red Devil", "8-pin x 3", 849.99000000000001, "330W", "16GB", "GDDR6" },
                    { new Guid("0f3f6e76-4d49-459d-af0b-f4ca67d11293"), "2420 MHz", "2680 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "Radeon RX 6700 XT", "PCIe 4.0 x16", new DateOnly(2022, 3, 18), "256 Bit", "Radeon RX 6700 XT MECH 2X", "8-pin x 1", 459.99000000000001, "230W", "12GB", "GDDR6" },
                    { new Guid("2a481176-a2fa-4673-b1ca-feb4040ee676"), "2580 MHz", "2670 MHz", "XFX", "DisplayPort x 2 (v2.0) HDMI™ x 1", "Radeon RX 6650 XT", "PCIe 4.0 x16", new DateOnly(2023, 5, 10), "128 Bit", "Speedster MERC 310", "8-pin x 1", 349.99000000000001, "180W", "8GB", "GDDR6" },
                    { new Guid("6df2957d-a948-4506-9c86-b26d176f63cc"), "2400 MHz", "2600 MHz", "Gigabyte", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7900 XTX", "PCIe 5.0 x16", new DateOnly(2023, 12, 15), "384 Bit", "AORUS MASTER", "8-pin x 3", 1399.99, "350W", "24GB", "GDDR6" },
                    { new Guid("7307efc2-8541-4a1e-b3f4-1a664629d684"), "1580 MHz", "1830 MHz", "Inno3D", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3070 Ti", "PCIe 4.0 x16", new DateOnly(2021, 6, 1), "256 Bit", "Twin X2 OC", "8-pin x 2", 499.99000000000001, "290W", "8GB", "GDDR6X" },
                    { new Guid("946b7c5a-2e29-4f51-b9c3-de11c818630d"), "1720 MHz", "1807 MHz", "PNY", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3050", "PCIe 4.0 x16", new DateOnly(2021, 1, 27), "128 Bit", "XLR8 RTX 3050", "6-pin x 1", 279.99000000000001, "130W", "8GB", "GDDR6" },
                    { new Guid("b7c2d317-ed38-4d0c-9e17-c9fb58a11878"), "2475 MHz", "2490 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX™ 4070", "PCIe 5.0 x16", new DateOnly(2023, 11, 15), "192 Bit", "VENTUS", "8-pin + 6-pin", 499.99000000000001, "200W", "12GB", "GDDR6X" },
                    { new Guid("ce532c24-81d2-4258-9212-a396d17265b7"), "1410 MHz", "1700 MHz", "EVGA", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3060 Ti", "PCIe 4.0 x16", new DateOnly(2021, 11, 17), "256 Bit", "GeForce RTX 3060 Ti", "8-pin x 1", 399.99000000000001, "200W", "8GB", "GDDR6" },
                    { new Guid("d51cc6cc-a02b-4980-8513-62177e751fdc"), "2530 MHz", "2575 MHz", "ASUS", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 4070 Ti", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "TUF Gaming", "8-pin x 2", 799.99000000000001, "285W", "10GB", "GDDR6X" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPUs");

            migrationBuilder.DropTable(
                name: "GPUs");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
