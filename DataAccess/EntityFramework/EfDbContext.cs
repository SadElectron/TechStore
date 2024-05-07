using Entities;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DataAccess.EntityFramework
{
    public class EfDbContext : DbContext
    {

        public DbSet<GPU> GPUs { get; set; }
        public DbSet<CPU> CPUs { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            optionsBuilder.UseSqlite($"Data Source=../DataAccess/Application.db;Cache=Shared", options =>
            {

            });

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>().HasOne(i => i.Thumbnail)
                .WithOne(t => t.Image)
                .HasForeignKey<Thumbnail>(t => t.ImageId)
                .IsRequired(false);

            modelBuilder.Entity<CPU>().HasData(cpuArray);

            modelBuilder.Entity<GPU>().HasData(gpuArray);
        }


        private static readonly CPU[] cpuArray = new CPU[]
        {
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i3-12100",
                CoreCount = 4,
                ThreadCount = 8,
                BaseClockSpeed = "3.3 GHz",
                MaxTurboFrequency = "4.3 GHz",
                CacheSize = "12 MB",
                SocketType = "LGA 1700",
                IntegratedGraphics = "Intel UHD Graphics 730",
                MemorySupport = "DDR4-3200",
                Tdp = "60W",
                LaunchDate = new(2022,1,18),
                Price = 129.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 3 5300G",
                CoreCount = 4,
                ThreadCount = 8,
                BaseClockSpeed = "4.0 GHz",
                MaxTurboFrequency = "4.6 GHz",
                CacheSize = "40 MB",
                SocketType = "AM4",
                IntegratedGraphics = "AMD Radeon Graphics Vega 6",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 4, 19),
                Price = 149.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i5-12400",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "2.5 GHz",
                MaxTurboFrequency = "4.4 GHz",
                CacheSize = "18 MB",
                SocketType = "LGA 1700",
                IntegratedGraphics = "Intel UHD Graphics 770",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 1, 18),
                Price = 229.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 5 5600X",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "3.7 GHz",
                MaxTurboFrequency = "4.6 GHz",
                CacheSize = "38 MB",
                SocketType = "AM4",
                IntegratedGraphics = "None",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 4, 19),
                Price = 299.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i7-12700K",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "3.6 GHz",
                MaxTurboFrequency = "5.2 GHz",
                CacheSize = "25 MB",
                SocketType = "LGA 1700",
                IntegratedGraphics = "Intel UHD Graphics 770",
                MemorySupport = "DDR4-3200",
                Tdp = "125W",
                LaunchDate = new (2022, 1, 18),
                Price = 439.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 9 5900X",
                CoreCount = 12,
                ThreadCount = 24,
                BaseClockSpeed = "3.7 GHz",
                MaxTurboFrequency = "4.8 GHz",
                CacheSize = "64 MB",
                SocketType = "AM4",
                IntegratedGraphics = "None",
                MemorySupport = "DDR4-3200",
                Tdp = "105W",
                LaunchDate = new (2020, 11, 19),
                Price = 549.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i9-11900K",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "3.5 GHz",
                MaxTurboFrequency = "5.3 GHz",
                CacheSize = "20 MB",
                SocketType = "LGA 1200",
                IntegratedGraphics = "Intel UHD Graphics 770",
                MemorySupport = "DDR4-3200",
                Tdp = "125W",
                LaunchDate = new (2021, 3, 18),
                Price = 499.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 5 7600",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "3.5 GHz",
                MaxTurboFrequency = "4.5 GHz",
                CacheSize = "32 MB",
                SocketType = "AM4",
                IntegratedGraphics = "AMD Radeon Graphics",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 4, 19),
                Price = 229.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i5-12400F",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "2.5 GHz",
                MaxTurboFrequency = "4.4 GHz",
                CacheSize = "18 MB",
                SocketType = "LGA 1700",
                IntegratedGraphics = "None",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 1, 18),
                Price = 199.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 3 5500",
                CoreCount = 6,
                ThreadCount = 12,
                BaseClockSpeed = "3.6 GHz",
                MaxTurboFrequency = "4.4 GHz",
                CacheSize = "19 MB",
                SocketType = "AM4",
                IntegratedGraphics = "AMD Radeon Graphics",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 4, 19),
                Price = 159.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "Intel",
                ModelName = "Core i3-12100F",
                CoreCount = 4,
                ThreadCount = 8,
                BaseClockSpeed = "3.3 GHz",
                MaxTurboFrequency = "4.3 GHz",
                CacheSize = "12 MB",
                SocketType = "LGA 1700",
                IntegratedGraphics = "None",
                MemorySupport = "DDR4-3200",
                Tdp = "60W",
                LaunchDate = new (2022, 1, 18),
                Price = 109.99
            },
            new CPU()
            {
                Id = Guid.NewGuid(),
                Brand = "AMD",
                ModelName = "Ryzen 7 5700G",
                CoreCount = 8,
                ThreadCount = 16,
                BaseClockSpeed = "3.8 GHz",
                MaxTurboFrequency = "4.6 GHz",
                CacheSize = "36 MB",
                SocketType = "AM4",
                IntegratedGraphics = "AMD Radeon Graphics Vega 8",
                MemorySupport = "DDR4-3200",
                Tdp = "65W",
                LaunchDate = new (2022, 4, 19),
                Price = 349.99
            }
        };

        private static readonly GPU[] gpuArray = new GPU[]
        {
            new GPU()
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
                Price = 499.99
            },
            new GPU()
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
                Price = 1399.99
            },
            new GPU()
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
                Price = 799.99
            },
            new GPU()
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
                Price = 849.99
            },
            new GPU()
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
                Price = 399.99
            },
                        new GPU()
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
                Price = 349.99
            },
            new GPU()
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
                Price = 279.99
            },
                        new GPU()
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
                Price = 229.99
            },
            new GPU()
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
                Price = 499.99
            },
            new GPU()
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
                Price = 459.99
            }
        };


    }
}
