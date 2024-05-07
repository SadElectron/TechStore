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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    File = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thumbnails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ThumbnailImage = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thumbnails_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CPUs",
                columns: new[] { "Id", "BaseClockSpeed", "Brand", "CacheSize", "CoreCount", "IntegratedGraphics", "LaunchDate", "MaxTurboFrequency", "MemorySupport", "ModelName", "Price", "SocketType", "Tdp", "ThreadCount" },
                values: new object[,]
                {
                    { new Guid("3596c757-9089-44d3-b655-4d5553c667ac"), "3.7 GHz", "AMD", "38 MB", 6, "None", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 5 5600X", 299.99000000000001, "AM4", "65W", 12 },
                    { new Guid("49da81f8-7a24-4e77-97d9-9ac31a22240a"), "3.6 GHz", "Intel", "25 MB", 8, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "5.2 GHz", "DDR4-3200", "Core i7-12700K", 439.99000000000001, "LGA 1700", "125W", 16 },
                    { new Guid("6aa6d226-54f5-47fa-8303-11fc43d96280"), "3.5 GHz", "AMD", "32 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.5 GHz", "DDR4-3200", "Ryzen 5 7600", 229.99000000000001, "AM4", "65W", 12 },
                    { new Guid("70a8eb3a-6f3f-4938-9c2f-91e76bc46d62"), "4.0 GHz", "AMD", "40 MB", 4, "AMD Radeon Graphics Vega 6", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 3 5300G", 149.99000000000001, "AM4", "65W", 8 },
                    { new Guid("73d6e7cc-0383-4a73-8c82-e2f38f35bd7e"), "3.5 GHz", "Intel", "20 MB", 8, "Intel UHD Graphics 770", new DateOnly(2021, 3, 18), "5.3 GHz", "DDR4-3200", "Core i9-11900K", 499.99000000000001, "LGA 1200", "125W", 16 },
                    { new Guid("78f44946-8915-4e1b-b70a-09c64ceaecc8"), "3.3 GHz", "Intel", "12 MB", 4, "None", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100F", 109.98999999999999, "LGA 1700", "60W", 8 },
                    { new Guid("ab77f6a4-c1f1-4efa-afb1-35d4d516bd80"), "3.8 GHz", "AMD", "36 MB", 8, "AMD Radeon Graphics Vega 8", new DateOnly(2022, 4, 19), "4.6 GHz", "DDR4-3200", "Ryzen 7 5700G", 349.99000000000001, "AM4", "65W", 16 },
                    { new Guid("ae810360-3c64-4d4d-a8f7-9eb48f875415"), "3.6 GHz", "AMD", "19 MB", 6, "AMD Radeon Graphics", new DateOnly(2022, 4, 19), "4.4 GHz", "DDR4-3200", "Ryzen 3 5500", 159.99000000000001, "AM4", "65W", 12 },
                    { new Guid("b91b9e1a-5506-4898-8229-db7dfe17ab12"), "2.5 GHz", "Intel", "18 MB", 6, "Intel UHD Graphics 770", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400", 229.99000000000001, "LGA 1700", "65W", 12 },
                    { new Guid("ccec29fe-4d4e-4ae8-8d24-6dc199f8ff63"), "3.7 GHz", "AMD", "64 MB", 12, "None", new DateOnly(2020, 11, 19), "4.8 GHz", "DDR4-3200", "Ryzen 9 5900X", 549.99000000000001, "AM4", "105W", 24 },
                    { new Guid("e598782d-2a3e-4fd7-a6b6-bf647071ef26"), "2.5 GHz", "Intel", "18 MB", 6, "None", new DateOnly(2022, 1, 18), "4.4 GHz", "DDR4-3200", "Core i5-12400F", 199.99000000000001, "LGA 1700", "65W", 12 },
                    { new Guid("f5dd7ad4-e08d-4540-98b6-8175802c2ed0"), "3.3 GHz", "Intel", "12 MB", 4, "Intel UHD Graphics 730", new DateOnly(2022, 1, 18), "4.3 GHz", "DDR4-3200", "Core i3-12100", 129.99000000000001, "LGA 1700", "60W", 8 }
                });

            migrationBuilder.InsertData(
                table: "GPUs",
                columns: new[] { "Id", "BaseClockSpeed", "BoostClockSpeed", "Brand", "DisplayPorts", "GraphicEngine", "Interface", "LaunchDate", "MemoryBusWidth", "ModelName", "PowerConnectors", "Price", "Tdp", "VramSize", "VramType" },
                values: new object[,]
                {
                    { new Guid("31e32d9d-f66e-4eed-94d5-be6481e2362b"), "2530 MHz", "2575 MHz", "ASUS", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 4070 Ti", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "TUF Gaming", "8-pin x 2", 799.99000000000001, "285W", "10GB", "GDDR6X" },
                    { new Guid("5d7393c5-0bc1-41ae-862c-dad6aa9a4c31"), "2100 MHz", "2045 MHz", "ASRock", "DisplayPort x 2 (v1.4) HDMI™ x 2", "Radeon RX 6600", "PCIe 4.0 x16", new DateOnly(2021, 8, 10), "128 Bit", "Challenger D", "6-pin x 1", 229.99000000000001, "130W", "8GB", "GDDR6" },
                    { new Guid("703e7ce5-1448-4da5-a4de-d976bccdeb65"), "1410 MHz", "1700 MHz", "EVGA", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3060 Ti", "PCIe 4.0 x16", new DateOnly(2021, 11, 17), "256 Bit", "GeForce RTX 3060 Ti", "8-pin x 1", 399.99000000000001, "200W", "8GB", "GDDR6" },
                    { new Guid("8594bd27-c52f-481c-b4dd-e520014bdbde"), "2475 MHz", "2490 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX™ 4070", "PCIe 5.0 x16", new DateOnly(2023, 11, 15), "192 Bit", "VENTUS", "8-pin + 6-pin", 499.99000000000001, "200W", "12GB", "GDDR6X" },
                    { new Guid("b94980c3-7cb3-4386-bf9d-0fdf857f04f1"), "2300 MHz", "2700 MHz", "PowerColor", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7800 XT", "PCIe 5.0 x16", new DateOnly(2023, 12, 1), "256 Bit", "Red Devil", "8-pin x 3", 849.99000000000001, "330W", "16GB", "GDDR6" },
                    { new Guid("c51e4939-cb61-4730-9804-b57b192b0ed9"), "2420 MHz", "2680 MHz", "MSI", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "Radeon RX 6700 XT", "PCIe 4.0 x16", new DateOnly(2022, 3, 18), "256 Bit", "Radeon RX 6700 XT MECH 2X", "8-pin x 1", 459.99000000000001, "230W", "12GB", "GDDR6" },
                    { new Guid("ce785a61-6a6e-4a42-879c-734bfd180177"), "2580 MHz", "2670 MHz", "XFX", "DisplayPort x 2 (v2.0) HDMI™ x 1", "Radeon RX 6650 XT", "PCIe 4.0 x16", new DateOnly(2023, 5, 10), "128 Bit", "Speedster MERC 310", "8-pin x 1", 349.99000000000001, "180W", "8GB", "GDDR6" },
                    { new Guid("d3939441-ca31-424d-9b7b-38de273859a2"), "1580 MHz", "1830 MHz", "Inno3D", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3070 Ti", "PCIe 4.0 x16", new DateOnly(2021, 6, 1), "256 Bit", "Twin X2 OC", "8-pin x 2", 499.99000000000001, "290W", "8GB", "GDDR6X" },
                    { new Guid("f7867547-a9df-43b9-96d1-ad65d75c9858"), "2400 MHz", "2600 MHz", "Gigabyte", "DisplayPort x 3 (v2.1) HDMI™ x 1", "Radeon RX 7900 XTX", "PCIe 5.0 x16", new DateOnly(2023, 12, 15), "384 Bit", "AORUS MASTER", "8-pin x 3", 1399.99, "350W", "24GB", "GDDR6" },
                    { new Guid("fcffd450-7198-4ef0-bc63-a007c6343916"), "1720 MHz", "1807 MHz", "PNY", "DisplayPort x 3 (v1.4a) HDMI™ x 1", "GeForce RTX 3050", "PCIe 4.0 x16", new DateOnly(2021, 1, 27), "128 Bit", "XLR8 RTX 3050", "6-pin x 1", 279.99000000000001, "130W", "8GB", "GDDR6" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Thumbnails_ImageId",
                table: "Thumbnails",
                column: "ImageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPUs");

            migrationBuilder.DropTable(
                name: "GPUs");

            migrationBuilder.DropTable(
                name: "Thumbnails");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
