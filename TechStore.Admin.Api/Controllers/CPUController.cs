using Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Validation;
using TechStore.Areas.Admin.Models.Cpu;

namespace TechStore.Areas.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class CPUController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<CPUController> _logger;
    private readonly CPUValidator _cpuValidator;
    public CPUController(ICPUService cpuService, ILogger<CPUController> logger, CPUValidator cpuValidator)
    {
        _cpuService = cpuService;
        _logger = logger;
        _cpuValidator = cpuValidator;
    }
    // GET: Admin/CPUController
    public async Task<IActionResult> Index()
    {
        var cpus = await _cpuService.GetAllAsync();
        return View(cpus);
    }


    // GET: Admin/CPUController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Admin/CPUController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromServices] IImageService imageService, CpuModel cpuModel, IFormFileCollection Images)
    {
        if (cpuModel.Cpu == null)
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
    public IActionResult Details([ModelBinder(BinderType = typeof(CpuModelBinder))] CpuModel cpuModel)
    {
        if (cpuModel.Cpu == null)
            return BadRequest();
        var validationResult = _cpuValidator.Validate(cpuModel.Cpu);
        if (validationResult.IsValid)
        {

            return View(new CpuModel { Cpu = cpuModel.Cpu, Images = cpuModel.Images });
        }
        return BadRequest(validationResult.Errors);
    }

    // GET: Admin/CPU/Update/5
    public async Task<IActionResult> Update([FromServices] IImageService imageService, Guid id)
    {

        var cpu = await _cpuService.GetByIdAsync(id);
        if (cpu == null)
        {
            return NotFound();
        }
        var images = await imageService.GetAllAsync(i => i.ProductId == id);
        var model = new CpuModel { Cpu = cpu, Images = images };

        return View(model);
    }

    // POST: Admin/CPUController/Update/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(CpuModel cpuModel)
    {
        if (cpuModel.Cpu == null)
            return BadRequest();
        var validationResult = _cpuValidator.Validate(cpuModel.Cpu);
        if (validationResult.IsValid)
        {
            await _cpuService.UpdateAsync(cpuModel.Cpu);


            return RedirectToAction("Details", cpuModel.Cpu);
        }
        return BadRequest(validationResult.ToString());
    }

    // GET: Admin/CPUController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var cpu = await _cpuService.GetByIdAsync(id);
        if (cpu == null)
        {
            return BadRequest();
        }

        return View(cpu);
    }

    // POST: Admin/CPUController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromServices] IImageService imageService, CPU cpu)
    {
        var validationResult = _cpuValidator.Validate(cpu);
        if (validationResult.IsValid)
        {
            var images = await imageService.GetAllAsync(i => i.ProductId == cpu.Id);
            if (images.Count > 0)
            {
                bool imageDeleteResult = await imageService.DeleteAllAsync(cpu.Id);
            }

            await _cpuService.DeleteAsync(cpu);
            return RedirectToAction("Index");
        }
        return BadRequest(validationResult.ToString());
    }
}

