using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class GPU : IEntity, IProduct
    {
        [Key]
        public Guid Id { get; set; }


        public required string Brand { get; set; }


        public required string ModelName { get; set; }


        public required string GraphicEngine { get; set; }


        public required string VramSize { get; set; }


        public required string VramType { get; set; }


        public required string BaseClockSpeed { get; set; }

        public required string BoostClockSpeed { get; set; }


        public required string MemoryBusWidth { get; set; }

        public required string Tdp { get; set; }


        public required string PowerConnectors { get; set; }


        public required string Interface { get; set; }


        public required string DisplayPorts { get; set; }

        public DateOnly LaunchDate { get; set; }

        public double Price { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
