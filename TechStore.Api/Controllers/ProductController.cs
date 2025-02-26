using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Services.Abstract;
using Services.Concrete;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Product;
using TechStore.Api.Models;
using TechStore.Api.Models.Product;


namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
    {
        _productService = productService;
        _mapper = mapper;
        _logger = logger;
    }
    // READ
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        try
        {
            var entity = await _productService.GetAsNoTrackingAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<ProductDto>(entity);
            return Ok(dto);
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpGet("Get/{page}/{count}")]
    public async Task<IActionResult> GetProducts(int page, int count, [FromServices] IValidator<(int, int)> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync((page, count));
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var entities = await _productService.GetAllAsync(page, count);
            var dtos = _mapper.Map<ICollection<ProductDto>>(entities);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("Get/Count")]
    public async Task<IActionResult> GetEntryCount()
    {
        try
        {
            var entityCount = await _productService.GetEntryCountAsync();
            return Ok(entityCount);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("Get/Count/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId, [FromServices] IValidator<Guid> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(categoryId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var productCount = await _productService.GetProductCountAsync(categoryId);
            return Ok(productCount);
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProduct(CreateProductModel model, [FromServices] IValidator<CreateProductModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var entity = _mapper.Map<Product>(model);

            var createEntityResult = await _productService.AddAsync(entity);
            if (!createEntityResult.IsSuccessful)
            {
                return BadRequest(new { message = createEntityResult.Message });
            }
            var addedEntityDto = _mapper.Map<ProductDto>(createEntityResult.Entity);
            return Ok(addedEntityDto);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in ProductController.CreateProduct {ex.Message}");
            return BadRequest();
        }

    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model, [FromServices] IValidator<UpdateProductModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var entity = await _productService.GetAsNoTrackingAsync(model.Id);
            var product = _mapper.Map(model, entity);
            var updatedEntity = await _productService.UpdateAsync(product!);
            var entityDto = _mapper.Map<ProductDto>(updatedEntity);
            return Ok(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.UpdateProduct {ex.Message}");
            return BadRequest();
        }
    }
    [HttpPut("update/productorder")]
    public async Task<IActionResult> UpdateProductOrder([FromBody] UpdateProductOrderModel model, [FromServices] IValidator<UpdateProductOrderModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }

            var updatedEntity = await _productService.UpdateProductOrderAsync(model.Id, model.ProductOrder);
            var entityDto = _mapper.Map<ProductDto>(updatedEntity);
            return Ok(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.UpdateProductOrder {ex.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteProductModel model, [FromServices] IValidator<DeleteProductModel> validator)
    {

        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            EntityDeleteResult deleteResult = await _productService.DeleteAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.Delete {ex.Message}");
            return BadRequest();
        }
    }
}
