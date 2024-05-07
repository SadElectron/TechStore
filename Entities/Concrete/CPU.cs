using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CPU : IEntity, IProduct
    {
        [Key]
        public Guid Id { get; set; }

        public required string Brand { get; set; }

        public required string ModelName { get; set; }

        public int CoreCount { get; set; }

        public int ThreadCount { get; set; }
        
        public required string BaseClockSpeed { get; set; } 

        public required string MaxTurboFrequency { get; set; } 
        
        public required string CacheSize { get; set; }
        
        public required string SocketType { get; set; }

        public required string IntegratedGraphics { get; set; }

        public required string MemorySupport { get; set; }

        public required string Tdp { get; set; } 

        public DateOnly LaunchDate { get; set; }

        public double Price { get; set; }

        // Add additional fields as needed, based on your specific requirements
    }
}
