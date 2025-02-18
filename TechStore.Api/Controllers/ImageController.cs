using AutoMapper;
using Core.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IImageService imageService, IMapper mapper, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("get/images/{productId}")]
    public async Task<IActionResult> GetAll(Guid productId)
    {

        var images = await _imageService.GetAllAsNoTrackingAsync(productId);
        if (images.Count == 0) return NotFound();
        return Ok(images);

    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateImage([FromForm] Guid productId, List<IFormFile> files)
    {
        var addedImages = await _imageService.BulkAddAsync(files, productId);
        return CreatedAtAction("GetAll", routeValues: new { productId = productId }, null);
    }

    [HttpPut("update/order/{imageId}")]
    public async Task<IActionResult> UpdateOrder(Guid imageId, int newOrder)
    {
        var result = await _imageService.UpdateOrderAsync(imageId, newOrder);
        if (result == null) return BadRequest();

        return Ok(result);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        EntityDeleteResult deleteResult = await _imageService.DeleteAndReorderAsync(id);
        return deleteResult.IsSuccessful ? Ok() : NotFound();
    }
}
