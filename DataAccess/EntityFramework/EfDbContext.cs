using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection.Metadata;


namespace DataAccess.EntityFramework;

public class EfDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserData> UserData { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Order> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Detail> Details { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(b => b.Log(
                (CoreEventId.StartedTracking, LogLevel.Information),
                (RelationalEventId.CommandExecuted, LogLevel.Information))).LogTo(Console.WriteLine, new[] {
            CoreEventId.StartedTracking,
            RelationalEventId.CommandExecuted
        },
            LogLevel.Debug, DbContextLoggerOptions.SingleLine);

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlite($"Data Source=../DataAccess/Application.db;Cache=Shared", options =>
        {

        });

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        /*modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(cpuProducts);
        modelBuilder.Entity<Property>().HasData(cpuProps);
        modelBuilder.Entity<Detail>().HasData(details);*/
        var cpuArray = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 9 7950X3D",
                CoreCount = 16,
                ThreadCount = 32,
                BaseClockSpeed = "4.2 GHz",
                MaxTurboFrequency = "Up to 5.7 GHz",
                CacheSize = "16 MB L2, 128 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "120W",
                LaunchDate = DateOnly.Parse("2023-02-28"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 525.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 9 7950X",
                CoreCount = 16,
                ThreadCount = 32,
                BaseClockSpeed = "4.5 GHz",
                MaxTurboFrequency = "Up to 5.7 GHz",
                CacheSize = "16 MB L2, 64 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "170W",
                LaunchDate = DateOnly.Parse("2022-09-27"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 519.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 9 7900X3D",
                CoreCount = 12,
                ThreadCount = 24,
                BaseClockSpeed = "4.4 GHz",
                MaxTurboFrequency = "Up to 5.6 GHz",
                CacheSize = "12 MB L2, 128 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "120W",
                LaunchDate = DateOnly.Parse("2023-02-28"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 399.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 9 7900X",
                CoreCount = 12,
                ThreadCount = 24,
                BaseClockSpeed = "4.7 GHz",
                MaxTurboFrequency = "Up to 5.6 GHz",
                CacheSize = "12 MB L2, 64 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "170W",
                LaunchDate = DateOnly.Parse("2022-09-27"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 350.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 9 7900",
                CoreCount = 12,
                ThreadCount = 24,
                BaseClockSpeed = "3.7 GHz",
                MaxTurboFrequency = "Up to 5.4 GHz",
                CacheSize = "12 MB L2, 64 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "65W",
                LaunchDate = DateOnly.Parse("2023-01-14"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 369.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 7 7800X3D",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "4.2 GHz",
                MaxTurboFrequency = "Up to 5 GHz",
                CacheSize = "8 MB L2, 96 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "120W",
                LaunchDate = DateOnly.Parse("2023-04-06"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 365.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 7 7700X",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "4.5 GHz",
                MaxTurboFrequency = "Up to 5.4 GHz",
                CacheSize = "8 MB L2, 32 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "105W",
                LaunchDate = DateOnly.Parse("2022-09-27"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 277.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 7 7700",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "3.8 GHz",
                MaxTurboFrequency = "Up to 5.3 GHz",
                CacheSize = "8 MB L2, 32 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "65W",
                LaunchDate = DateOnly.Parse("2023-01-14"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 30,
                Price = 280.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 5 7600X",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "4.7 GHz",
                MaxTurboFrequency = "Up to 5.3 GHz",
                CacheSize = "6 MB L2, 32 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "105W",
                LaunchDate = DateOnly.Parse("2022-09-27"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 50,
                Price = 195.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 5 7600",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "3.8 GHz",
                MaxTurboFrequency = "Up to 5.1 GHz",
                CacheSize = "6 MB L2, 32 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "AMD Radeon™ Graphics",
                MemorySupport = "DDR5",
                Tdp = "65W",
                LaunchDate = DateOnly.Parse("2023-01-14"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 50,
                Price = 180.0
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen™ 5 7500F",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "3.7 GHz",
                MaxTurboFrequency = "Up to 5 GHz",
                CacheSize = "6 MB L2, 32 MB L3",
                SocketType = "AM5",
                IntegratedGraphics = "Discrete Graphics Card Required",
                MemorySupport = "DDR5",
                Tdp = "65W",
                LaunchDate = DateOnly.Parse("2023-07-22"),
                EntryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SoldQuantity = 0,
                Stock = 50,
                Price = 150.0
            }
        };

        var gpuArray = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                Brand = "MSI",
                ModelName = "VENTUS",
                GraphicEngine = "GeForce RTX™ 4070",
                VramSize = "12GB",
                VramType = "GDDR6X",
                BaseClockSpeed = "2475 MHz",
                BoostClockSpeed = "2490 MHz",
                MemoryBusWidth = "192 Bit",
                Tdp = "200W",
                PowerConnectors = "8-pin + 6-pin",
                Interface = "PCIe 5.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2023,11,15),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 499.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "Gigabyte",
                ModelName = "AORUS MASTER",
                GraphicEngine = "Radeon RX 7900 XTX",
                VramSize = "24GB",
                VramType = "GDDR6",
                BaseClockSpeed = "2400 MHz",
                BoostClockSpeed = "2600 MHz",
                MemoryBusWidth = "384 Bit",
                Tdp = "350W",
                PowerConnectors = "8-pin x 3",
                Interface = "PCIe 5.0 x16",
                DisplayPorts = "DisplayPort x 3 (v2.1) HDMI™ x 1",
                LaunchDate = new DateOnly(2023, 12, 15),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 1399.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "ASUS",
                ModelName = "TUF Gaming",
                GraphicEngine = "GeForce RTX 4070 Ti",
                VramSize = "10GB",
                VramType = "GDDR6X",
                BaseClockSpeed = "2530 MHz",
                BoostClockSpeed = "2575 MHz",
                MemoryBusWidth = "256 Bit",
                Tdp = "285W",
                PowerConnectors = "8-pin x 2",
                Interface = "PCIe 5.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2023, 12, 1),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 799.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "PowerColor",
                ModelName = "Red Devil",
                GraphicEngine = "Radeon RX 7800 XT",
                VramSize = "16GB",
                VramType = "GDDR6",
                BaseClockSpeed = "2300 MHz",
                BoostClockSpeed = "2700 MHz",
                MemoryBusWidth = "256 Bit",
                Tdp = "330W",
                PowerConnectors = "8-pin x 3",
                Interface = "PCIe 5.0 x16",
                DisplayPorts = "DisplayPort x 3 (v2.1) HDMI™ x 1",
                LaunchDate = new DateOnly(2023, 12, 1),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 849.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "EVGA",
                ModelName = "GeForce RTX 3060 Ti",
                GraphicEngine = "GeForce RTX 3060 Ti",
                VramSize = "8GB",
                VramType = "GDDR6",
                BaseClockSpeed = "1410 MHz",
                BoostClockSpeed = "1700 MHz",
                MemoryBusWidth = "256 Bit",
                Tdp = "200W",
                PowerConnectors = "8-pin x 1",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2021, 11, 17),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 399.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "XFX",
                ModelName = "Speedster MERC 310",
                GraphicEngine = "Radeon RX 6650 XT",
                VramSize = "8GB",
                VramType = "GDDR6",
                BaseClockSpeed = "2580 MHz",
                BoostClockSpeed = "2670 MHz",
                MemoryBusWidth = "128 Bit",
                Tdp = "180W",
                PowerConnectors = "8-pin x 1",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 2 (v2.0) HDMI™ x 1",
                LaunchDate = new DateOnly(2023, 5, 10),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 349.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "PNY",
                ModelName = "XLR8 RTX 3050",
                GraphicEngine = "GeForce RTX 3050",
                VramSize = "8GB",
                VramType = "GDDR6",
                BaseClockSpeed = "1720 MHz",
                BoostClockSpeed = "1807 MHz",
                MemoryBusWidth = "128 Bit",
                Tdp = "130W",
                PowerConnectors = "6-pin x 1",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2021, 1, 27),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 279.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "ASRock",
                ModelName = "Challenger D",
                GraphicEngine = "Radeon RX 6600",
                VramSize = "8GB",
                VramType = "GDDR6",
                BaseClockSpeed = "2100 MHz",
                BoostClockSpeed = "2045 MHz",
                MemoryBusWidth = "128 Bit",
                Tdp = "130W",
                PowerConnectors = "6-pin x 1",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 2 (v1.4) HDMI™ x 2",
                LaunchDate = new DateOnly(2021, 8, 10),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 229.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "Inno3D",
                ModelName = "Twin X2 OC",
                GraphicEngine = "GeForce RTX 3070 Ti",
                VramSize = "8GB",
                VramType = "GDDR6X",
                BaseClockSpeed = "1580 MHz",
                BoostClockSpeed = "1830 MHz",
                MemoryBusWidth = "256 Bit",
                Tdp = "290W",
                PowerConnectors = "8-pin x 2",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2021, 6, 1),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 499.99
            },
            new
            {
                Id = Guid.NewGuid(),
                Brand = "MSI",
                ModelName = "Radeon RX 6700 XT MECH 2X",
                GraphicEngine = "Radeon RX 6700 XT",
                VramSize = "12GB",
                VramType = "GDDR6",
                BaseClockSpeed = "2420 MHz",
                BoostClockSpeed = "2680 MHz",
                MemoryBusWidth = "256 Bit",
                Tdp = "230W",
                PowerConnectors = "8-pin x 1",
                Interface = "PCIe 4.0 x16",
                DisplayPorts = "DisplayPort x 3 (v1.4a) HDMI™ x 1",
                LaunchDate = new DateOnly(2022, 3, 18),
                EntryTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Price = 459.99
            }
        };
    }

    private static Category[] categories =
    [
        new Category { Id = Guid.NewGuid(), Order = 1, CategoryName = "Cpu", LastUpdate = DateTime.UtcNow },
        new Category { Id = Guid.NewGuid(), Order = 2, CategoryName = "Gpu", LastUpdate = DateTime.UtcNow }
    ];

    private static Property[] cpuProps =
    [
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 1, PropName = "Brand", LastUpdate= DateTime.UtcNow}, // 0
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 2, PropName = "Family", LastUpdate= DateTime.UtcNow}, //1
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 3, PropName = "Series", LastUpdate= DateTime.UtcNow}, //2
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 4, PropName = "Core count", LastUpdate= DateTime.UtcNow}, //3
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 5, PropName = "Thread count", LastUpdate= DateTime.UtcNow}, //4
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 6, PropName = "Base clock speed", LastUpdate = DateTime.UtcNow}, //5
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 7, PropName = "Max turbo frequency", LastUpdate = DateTime.UtcNow}, //6
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 8, PropName = "Socket type", LastUpdate = DateTime.UtcNow}, //7
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 9, PropName = "Integrated graphics", LastUpdate = DateTime.UtcNow}, //8  
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 10, PropName = "Memory support", LastUpdate = DateTime.UtcNow}, //9
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 11, PropName = "Tdp", LastUpdate = DateTime.UtcNow}, //10
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 12, PropName = "Launch date", LastUpdate = DateTime.UtcNow}, //11
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 13, PropName = "L1 cache", LastUpdate = DateTime.UtcNow}, //12
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 14, PropName = "L2 cache", LastUpdate = DateTime.UtcNow}, //13
        new Property { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 15, PropName = "L3 cache", LastUpdate = DateTime.UtcNow}, //14
    ];
    private static Product[] cpuProducts =
    [
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 1, ProductName = "AMD Ryzen™ 9 7950X3D", Stock = 30, SoldQuantity = 0, Price = 525.0, LastUpdate = DateTime.UtcNow }, //0
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 2, ProductName = "AMD Ryzen™ 9 7950X", Stock = 30, SoldQuantity = 0, Price = 519.0, LastUpdate = DateTime.UtcNow },//1
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 3, ProductName = "AMD Ryzen™ 9 7900X3D", Stock = 30, SoldQuantity = 0, Price = 399.0, LastUpdate = DateTime.UtcNow },//2
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 4, ProductName = "AMD Ryzen™ 9 7900X", Stock = 30, SoldQuantity = 0, Price = 350.0, LastUpdate = DateTime.UtcNow },//3
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 5, ProductName = "AMD Ryzen™ 9 7900", Stock = 30, SoldQuantity = 0, Price = 369.0, LastUpdate = DateTime.UtcNow },//4
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 6, ProductName = "AMD Ryzen™ 7 7800X3D", Stock = 30, SoldQuantity = 0, Price = 365.0, LastUpdate = DateTime.UtcNow },//5
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 7, ProductName = "AMD Ryzen™ 7 7700X", Stock = 30, SoldQuantity = 0, Price = 277.0, LastUpdate = DateTime.UtcNow },//6
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 8, ProductName = "AMD Ryzen™ 7 7700", Stock = 30, SoldQuantity = 0, Price = 280.0, LastUpdate = DateTime.UtcNow },//7
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 9, ProductName = "AMD Ryzen™ 5 7600X", Stock = 50, SoldQuantity = 0, Price = 195.0, LastUpdate = DateTime.UtcNow },//8
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 10, ProductName = "AMD Ryzen™ 5 7600", Stock = 50, SoldQuantity = 0, Price = 180.0, LastUpdate = DateTime.UtcNow },//9
        new Product { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Order = 11, ProductName = "AMD Ryzen™ 5 7500F", Stock = 50, SoldQuantity = 0, Price = 150.0, LastUpdate = DateTime.UtcNow }//10
    ];

    private static Detail[] details =
    [
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 1, PropertyId = cpuProps[0].Id, PropValue = "AMD", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 2, PropertyId = cpuProps[1].Id, PropValue = "Ryzen", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 3, PropertyId = cpuProps[2].Id, PropValue = "7000", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 4, PropertyId = cpuProps[3].Id, PropValue = "16", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 5, PropertyId = cpuProps[4].Id, PropValue = "32", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 6, PropertyId = cpuProps[5].Id, PropValue = "4.2 GHz", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 7, PropertyId = cpuProps[6].Id, PropValue = "5.7 GHz", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 8, PropertyId = cpuProps[7].Id, PropValue = "AM5", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 9, PropertyId = cpuProps[8].Id, PropValue = "AMD Radeon™ Graphics", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 10, PropertyId = cpuProps[9].Id, PropValue = "DDR5", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 11, PropertyId = cpuProps[10].Id, PropValue = "120W", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 12, PropertyId = cpuProps[11].Id, PropValue = "2023-02-28", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 13, PropertyId = cpuProps[13].Id, PropValue = "16 MB", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[0].Id, Order = 14, PropertyId = cpuProps[14].Id, PropValue = "128 MB", LastUpdate = DateTime.UtcNow},
       
        // AMD Ryzen™ 9 7950X
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 1, PropertyId = cpuProps[0].Id, PropValue = "AMD", LastUpdate = DateTime.UtcNow },
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 2, PropertyId = cpuProps[1].Id, PropValue = "Ryzen", LastUpdate = DateTime.UtcNow },
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 3, PropertyId = cpuProps[2].Id, PropValue = "7000", LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 4, PropertyId = cpuProps[3].Id, PropValue = "16" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 5, PropertyId = cpuProps[4].Id, PropValue = "32" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 6, PropertyId = cpuProps[5].Id, PropValue = "4.5 GHz" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 7, PropertyId = cpuProps[6].Id, PropValue = "5.7 GHz" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 8, PropertyId = cpuProps[7].Id, PropValue = "AM5" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 9, PropertyId = cpuProps[8].Id, PropValue = "AMD Radeon™ Graphics" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 10, PropertyId = cpuProps[9].Id, PropValue = "DDR5" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 11, PropertyId = cpuProps[10].Id, PropValue = "170W" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 12, PropertyId = cpuProps[11].Id, PropValue = "2022-09-27" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 13, PropertyId = cpuProps[13].Id, PropValue = "16 MB" , LastUpdate = DateTime.UtcNow},
        new Detail { Id = Guid.NewGuid(), ProductId = cpuProducts[1].Id, Order = 14, PropertyId = cpuProps[14].Id, PropValue = "64 MB" , LastUpdate = DateTime.UtcNow},

    ];
}
