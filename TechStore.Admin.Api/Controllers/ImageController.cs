using Entities.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Areas.Admin.Models;
using TechStore.Areas.Admin.Models.Image;

namespace TechStore.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
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
            if (image == null)
            {
                return NotFound();
            }
            await _imageService.DeleteAsync(id);
            return Ok();
        }


        // POST: Admin/Image/Create
        [HttpPost]
        public async Task<IActionResult> Create(Guid productId, IFormFileCollection files)
        {

            if (productId == Guid.Empty || files.Count == 0)
            {
                return BadRequest();
            }

            var imageList = new List<Image>();
            foreach (var file in files)
            {
                await using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);


                imageList.Add(new Image
                {
                    ProductId = productId,
                    File = memoryStream.ToArray(),
                    Thumbnail = null
                });

            }
            await _imageService.BulkAddAsync(imageList);
            return Created(string.Empty, imageList);
        }
    }
}
