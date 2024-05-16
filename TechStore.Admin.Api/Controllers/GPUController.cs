using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using SQLitePCL;
using Services.Validation;
using TechStore.Areas.Admin.Models.Cpu;
using TechStore.Areas.Admin.Models.Gpu;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace TechStore.Areas.Admin.Controllers;

[ApiController]
[Route("[controller]")]
public class GPUController : ControllerBase
{
    private readonly IGPUService _gpuService;
    private readonly ILogger<GPUController> _logger;
    private readonly GPUValidator _gpuValidator;

    public GPUController(IGPUService gpuService, ILogger<GPUController> logger, GPUValidator gpuValidator)
    {
        _gpuService = gpuService;
        _logger = logger;
        _gpuValidator = gpuValidator;
    }

    // GET: GPU
    public async Task<IActionResult> Index()
    {
        var gpus = await _gpuService.GetAllAsync();
        return View(gpus);
    }

    // GET: GPU/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: GPU/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromServices] IImageService imageService, GpuModel gpuModel, IFormFileCollection Images)
    {
        var gpuValidationResult = _gpuValidator.Validate(gpuModel.Gpu);
        if (!gpuValidationResult.IsValid)
        {
            return BadRequest(gpuValidationResult.Errors.Select(f => f.ErrorMessage).ToList());
        }


        var addedGpu = await _gpuService.AddAsync(gpuModel.Gpu);
        if (Images.Count > 0)
        {
            var imageEntities = new List<Image>();
            foreach (var image in Images)
            {
                MemoryStream ms = new();
                image.CopyTo(ms);
                imageEntities.Add(new Image
                {
                    ProductId = addedGpu.Id,
                    File = ms.ToArray(),
                    Thumbnail = null
                });
            }
            await imageService.BulkAddAsync(imageEntities);
        }
        return RedirectToAction("Details", gpuModel.Gpu);


    }

    public async Task<IActionResult> DetailsAsync([FromServices] IImageService imageService, Guid id)
    {

        var gpu = await _gpuService.GetByIdAsync(id);
        var validationResult = _gpuValidator.Validate(gpu);
        if (validationResult.IsValid)
        {
            var images = await imageService.GetAllAsync(i => i.ProductId == gpu.Id);
            return View(new GpuModel { Gpu = gpu, Images = images });
        }
        return BadRequest(validationResult.Errors);
    }

    // GET: GPU/Update/5
    public async Task<IActionResult> UpdateAsync([FromServices] IImageService imageService, Guid id)
    {
        var gpu = await _gpuService.GetByIdAsync(id);
        if (gpu == null)
        {
            return NotFound();
        }
        var images = await imageService.GetAllAsync(i => i.ProductId == id);
        var model = new GpuModel { Gpu = gpu, Images = images };

        return View(model);
    }

    // POST: GPU/Update/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAsync(GpuModel gpuModel)
    {
        var validationResult = _gpuValidator.Validate(gpuModel.Gpu);
        if (validationResult.IsValid)
        {
            await _gpuService.UpdateAsync(gpuModel.Gpu);
            return RedirectToAction("Details", new { id = gpuModel.Gpu.Id });
        }
        return BadRequest(validationResult.ToString());
    }

    // GET: GPU/Delete/5
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var gpuModel = new GpuModel
        {
            Gpu = await _gpuService.GetByIdAsync(id)
        };

        if (gpuModel.Gpu == null)
        {
            return BadRequest();
        }

        return View(gpuModel);
    }

    // POST: GPU/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync([FromServices] IImageService imageService, GpuModel gpuModel)
    {
        var validationResult = _gpuValidator.Validate(gpuModel.Gpu);
        if (validationResult.IsValid)
        {
            var images = await imageService.GetAllAsync(i => i.ProductId == gpuModel.Gpu.Id);
            if (images.Count > 0)
            {
                bool imageDeleteResult = await imageService.DeleteAllAsync(gpuModel.Gpu.Id);
            }

            await _gpuService.DeleteAsync(gpuModel.Gpu);
            return RedirectToAction("Index");
        }
        return BadRequest(validationResult.ToString());
    }
}
