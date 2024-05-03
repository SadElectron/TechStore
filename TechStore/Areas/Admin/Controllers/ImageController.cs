using Entities.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Areas.Admin.Models;
using TechStore.Areas.Admin.Models.Image;

namespace TechStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IImageService _imageService;
        private readonly ICPUService _cpuService;

        public ImageController(ILogger<ImageController> logger, IImageService imageService, ICPUService cpuService)
        {
            _logger = logger;
            _imageService = imageService;
            _cpuService = cpuService;
        }

        // GET: Admin/Image
        [HttpGet("[Controller]/id")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await _imageService.GetAsync(i => i.ImageId == id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image.File, "image/png");
        }



        // GET: Admin/Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Image/Update/5
        [HttpGet("Admin/Image/Update/{id}")]
        public async Task<IActionResult> Update(Guid id, string type)
        {
            var imageCpuModel = new ImageCpuModel();
            switch (type.ToLower())
            {
                case "cpu":
                    var cpu = await _cpuService.GetByIdAsync(id);
                    var images = await _imageService.GetAllAsync(i  => i.ProductId == id);
                    imageCpuModel.CpuId = cpu.Id;
                    imageCpuModel.Brand = cpu.Brand;
                    imageCpuModel.ModelName = cpu.ModelName;
                    imageCpuModel.Images = images;
                    return View(imageCpuModel);
                    
                case "gpu":

                    break;

                default:
                    break;
            }
            
            return BadRequest();
        }

        // POST: Admin/Image/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Image/Delete/5
        public IActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: Admin/Image/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
