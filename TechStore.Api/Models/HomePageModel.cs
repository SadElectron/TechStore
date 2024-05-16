using Entities.Concrete;

namespace TechStore.Areas.Products.Models
{
    public class HomePageModel
    {
        public List<CPU> Cpus{ get; set; } = new List<CPU>();
        public List<GPU> Gpus { get; set; } = new List<GPU>();
    }
}
