using AutoMapper;
using Core.Dtos;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Image;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Models;

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

    [HttpGet("{imageId}")]
    public async Task<IActionResult> GetImage(ImageIdModel model, IValidator<ImageIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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
    public async Task<IActionResult> UploadImage([FromForm] UploadImagesModel model, IValidator<UploadImagesModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderModel model, IValidator<UpdateOrderModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var result = await _imageService.UpdateOrderAsync(model.Id, model.NewOrder);
            if (result == null) return BadRequest();
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
    public async Task<IActionResult> DeleteImage(ImageIdModel model, IValidator<ImageIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            EntityDeleteResult deleteResult = await _imageService.DeleteAndReorderAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.UpdateOrder {ex.Message}");
            return Problem();
        }
    }
}
