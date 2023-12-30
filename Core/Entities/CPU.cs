using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Cpu
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        public int CoreCount { get; set; }

        [Required]
        public int ThreadCount { get; set; }

        [Required]
        public decimal BaseClockSpeed { get; set; } // Using decimal for precise representation

        public decimal MaxTurboFrequency { get; set; } // Using decimal for precise representation

        [Required]
        public int CacheSize { get; set; }

        [Required]
        public string SocketType { get; set; }

        public string IntegratedGraphics { get; set; }

        public string MemorySupport { get; set; }

        public decimal Tdp { get; set; } // Using decimal for precise representation

        public string LaunchDate { get; set; }

        // Add additional fields as needed, based on your specific requirements
    }
}
