using AutoMapper;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Image;

namespace TechStore.Api.Models;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/images")]
[Authorize("Admin")]
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

    [HttpGet("{imageId}")]
    [TypeFilter(typeof(ValidateByModelFilter<ImageIdModel>))]
    public async Task<IActionResult> GetImage(ImageIdModel model)
    {
        try
        {
            var image = await _imageService.GetAsNoTrackingAsync(model.Id);
            if (image == null) return NotFound();
            var dto = _mapper.Map<ImageDto>(image);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.GetImageAsync {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("create")]
    [TypeFilter(typeof(ValidateByModelFilter<UploadImagesModel>))]
    public async Task<IActionResult> UploadImage([FromForm] UploadImagesModel model)
    {
        try
        {
            var addedImages = await _imageService.BulkAddAsync(model.Files, model.ProductId);
            return Ok(_mapper.Map<List<ImageDto>>(addedImages));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.GetImageAsync {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("order")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateImageOrderModel>))]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateImageOrderModel model)
    {
        try
        {
            var result = await _imageService.UpdateOrderAsync(model.Id, model.NewOrder);
            var dto = _mapper.Map<ImageDto>(result);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.UpdateOrder {ex.Message}");
            return Problem();
        }

    }

    [HttpDelete("{imageId}")]
    [TypeFilter(typeof(ValidateByModelFilter<ImageIdModel>))]
    public async Task<IActionResult> DeleteImage(ImageIdModel model)
    {
        try
        {
            EntityDeleteResult deleteResult = await _imageService.DeleteAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.UpdateOrder {ex.Message}");
            return Problem();
        }
    }
}
