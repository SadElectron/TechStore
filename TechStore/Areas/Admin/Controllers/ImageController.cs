using Entities.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Abstract;
using System.Drawing;
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



        // GET: Admin/Image/Create/{id}
        public IActionResult Create(Guid id, string productType)
        {
            if (!id.Equals(Guid.Empty))
            {
                var image = new ImageCreateModel(id, false, productType);
                
                return View(image);
            }
            return BadRequest();
        }

        // POST: Admin/Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageCreateModel collection, IFormFileCollection Files)
        {
            if (!collection.ProductId.Equals(Guid.Empty))
            {
                
                foreach (var file in Files)
                {
                    var meoryStream = new MemoryStream();
                    await file.CopyToAsync(meoryStream);
                    var imageArray = meoryStream.ToArray();
                    var image = new Entities.Concrete.Image { 
                        ProductId = collection.ProductId,
                        Type = collection.ImageType,
                        File = imageArray
                    };
                    await _imageService.AddAsync(image);
                    
                }
                return RedirectToAction("Update", new {id = collection.ProductId, productType = collection.ProductType});
            }

            return BadRequest(collection);
        }

        // GET: Admin/Image/Update/5
        [HttpGet("Admin/Image/Update/{id}")]
        public async Task<IActionResult> Update(Guid id, string productType)
        {
            var imageUpdateModel = new ImageUpdateModel();
            switch (productType.ToLower())
            {
                case "cpu":
                    var cpu = await _cpuService.GetByIdAsync(id);
                    var images = await _imageService.GetAllAsync(i  => i.ProductId == id);
                    imageUpdateModel.ProductId = id;
                    imageUpdateModel.ProductName = $"{cpu.Brand} {cpu.ModelName}";
                    imageUpdateModel.ProductType = productType;
                    imageUpdateModel.Images = images;
                    return View(imageUpdateModel);
                    
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
