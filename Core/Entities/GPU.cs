using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class GPU
    {
        public Guid ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ChipsetBrand { get; set; }
        public string Chipset { get; set; }
        public int Memory { get; set; }
        public int Clock { get; set; }
        public decimal Price { get; set; }


    }
}
