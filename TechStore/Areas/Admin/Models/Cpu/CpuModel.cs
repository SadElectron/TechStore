using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace TechStore.Areas.Admin.Models.Cpu
{
    
    public class CpuModel
    {
        public CPU? Cpu { get; set; }
        public List<Entities.Concrete.Image>? Images { get; set; }
    }
}
