using Entities.Concrete;

namespace TechStore.Areas.Admin.Models.Cpu
{
    public class CpuModel
    {
        public CPU? Cpu { get; set; }
        public List<Entities.Concrete.Image>? Images { get; set; }
    }
}
