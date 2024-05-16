using Microsoft.AspNetCore.Mvc;
using Services.Validation;
using TechStore.Admin.Api.Models.Admin;
using TechStore.Areas.Admin.Models.Cpu;

namespace TechStore.Admin.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{

    public AdminController()
    {

    }
    public async Task<IActionResult> Create([FromServices] IImageService imageService, ProductModel productModel, IFormFileCollection Images)
    {
        if (productModel.Cpu == null)
            return BadRequest();

        var cpuValidationResult = _cpuValidator.Validate(cpuModel.Cpu);
        if (cpuValidationResult.IsValid)
        {
            var addedCpuModel = new CpuModel();
            addedCpuModel.Cpu = await _cpuService.AddAsync(cpuModel.Cpu);
            if (Images != null)
            {
                var imageEntities = new List<Image>();
                foreach (var image in Images)
                {
                    MemoryStream ms = new();
                    image.CopyTo(ms);
                    imageEntities.Add(new Image
                    {
                        ProductId = addedCpuModel.Cpu.Id,
                        File = ms.ToArray(),
                        Thumbnail = null
                    });
                }
                addedCpuModel.Images = await imageService.BulkAddAsync(imageEntities);
            }
            return RedirectToAction("Details", addedCpuModel.Cpu);

        }

        return BadRequest(cpuValidationResult.Errors.Select(f => f.ErrorMessage).ToList());

    }
}

