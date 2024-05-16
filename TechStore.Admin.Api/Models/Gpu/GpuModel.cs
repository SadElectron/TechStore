using Entities.Concrete;

namespace TechStore.Areas.Admin.Models.Gpu;
public class GpuModel
{
    public GPU? Gpu { get; set; }
    public List<Entities.Concrete.Image>? Images { get; set; }
}
