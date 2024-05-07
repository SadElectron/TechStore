using Entities.Abstract;
using Entities.Concrete;
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

        public ImageController(ILogger<ImageController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        // GET: Admin/Image
        [HttpGet("[Controller]/id")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await _imageService.GetAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image.File, "image/png");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var image = await _imageService.GetAsync(id);
            if(image == null)
            {
                return NotFound();
            }
            await _imageService.DeleteAsync(id);
            return Ok();
        }


        // POST: Admin/Image/Create
        [HttpPost]
        public async Task<IActionResult> Create(Guid productId, IFormFileCollection Files)
        {
            if (!productId.Equals(Guid.Empty) && Files.Count > 0)
            {
                var imageList = new List<Image>();
                foreach (var file in Files)
                {
                    var meoryStream = new MemoryStream();
                    await file.CopyToAsync(meoryStream);
                    var imageArray = meoryStream.ToArray();
                    imageList.Add(new Image { 
                        ProductId = productId,
                        File = imageArray,
                        Thumbnail = null
                    });
                                     
                }
                await _imageService.BulkAddAsync(imageList);
                return Created(string.Empty, imageList);
            }

            return BadRequest();
        }
        
    }
}
