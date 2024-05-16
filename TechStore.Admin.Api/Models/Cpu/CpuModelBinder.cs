using Entities.Concrete;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Abstract;

namespace TechStore.Areas.Admin.Models.Cpu
{
    public class CpuModelBinder : IModelBinder
    {
        private readonly IImageService _imageService;

        public CpuModelBinder(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
                
                var cpuModel = new CpuModel();
                var cpu = new CPU {
                    Id = Guid.Parse(bindingContext.ValueProvider.GetValue("Id").FirstValue),
                    Brand = bindingContext.ValueProvider.GetValue("Brand").FirstValue,
                    ModelName = bindingContext.ValueProvider.GetValue("ModelName").FirstValue,
                    CoreCount = Convert.ToInt32(bindingContext.ValueProvider.GetValue("CoreCount").FirstValue),
                    ThreadCount = Convert.ToInt32(bindingContext.ValueProvider.GetValue("ThreadCount").FirstValue),
                    BaseClockSpeed = bindingContext.ValueProvider.GetValue("BaseClockSpeed").FirstValue,
                    MaxTurboFrequency = bindingContext.ValueProvider.GetValue("MaxTurboFrequency").FirstValue,
                    CacheSize = bindingContext.ValueProvider.GetValue("CacheSize").FirstValue,
                    SocketType = bindingContext.ValueProvider.GetValue("SocketType").FirstValue,
                    IntegratedGraphics = bindingContext.ValueProvider.GetValue("IntegratedGraphics").FirstValue,
                    MemorySupport = bindingContext.ValueProvider.GetValue("MemorySupport").FirstValue,
                    Tdp = bindingContext.ValueProvider.GetValue("Tdp").FirstValue,
                    LaunchDate = DateOnly.Parse(bindingContext.ValueProvider.GetValue("LaunchDate").FirstValue),
                    Price = Convert.ToDouble(bindingContext.ValueProvider.GetValue("Price").FirstValue)
                };
            cpuModel.Cpu = cpu;
            cpuModel.Images = await _imageService.GetAllAsync(i => i.ProductId == cpu.Id);

            bindingContext.Result = ModelBindingResult.Success(cpuModel);
  
        }
    }
}
