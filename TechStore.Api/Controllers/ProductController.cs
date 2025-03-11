using AutoMapper;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Product;


namespace TechStore.Api.Controllers;
[Authorize("Admin")]
[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/products")]
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
    [HttpGet("{productId}")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> GetProduct(ProductIdModel model, ProductIdValidator validator)
    {
        try
        {
            var entity = await _productService.GetAsNoTrackingAsync(model.Id);
            var dto = _mapper.Map<ProductDto>(entity);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.GetProduct {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{page}/{count}")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductPageModel>))]
    public async Task<IActionResult> GetProducts(ProductPageModel model)
    {
        try
        {
            var entities = await _productService.GetAllAsync(model.Page, model.Count);
            var dtos = _mapper.Map<ICollection<ProductDto>>(entities);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.GetProducts {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("Count")]
    public async Task<IActionResult> GetEntryCount()
    {
        try
        {
            var entityCount = await _productService.GetEntryCountAsync();
            return Ok(entityCount);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.GetEntryCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{productId}/images")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> GetProductImages(ProductIdModel model, [FromServices] IImageService imageService)
    {
        try
        {
            var images = await imageService.GetAllAsNoTrackingAsync(model.Id);
            if (images.Count == 0) return NotFound();
            var imageDtos = _mapper.Map<List<ImageDto>>(images);
            return Ok(imageDtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ImageController.GetAll {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{productId}/details")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> GetProductDetails(ProductIdModel model, [FromServices] IDetailService detailService)
    {
        try
        {
            var propertyValue = await detailService.GetProductDetailsAsync(model.Id);
            if (propertyValue.Count == 0)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<List<DetailDto>>(propertyValue);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetProductDetails {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("Create")]
    [TypeFilter(typeof(ValidateByModelFilter<CreateProductModel>))]
    public async Task<IActionResult> CreateProduct(CreateProductModel model)
    {
        try
        {
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
            return Problem();
        }

    }

    [HttpPut("Update")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateProductModel>))]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
    {
        try
        {
            var entity = await _productService.GetAsNoTrackingAsync(model.Id);
            var product = _mapper.Map(model, entity);
            var updatedEntity = await _productService.UpdateAsync(product!);
            var entityDto = _mapper.Map<ProductDto>(updatedEntity);
            return Ok(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.UpdateProduct {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("update/productorder")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateProductOrderModel>))]
    public async Task<IActionResult> UpdateProductOrder([FromBody] UpdateProductOrderModel model)
    {
        try
        {
            var updatedEntity = await _productService.UpdateProductOrderAsync(model.Id, model.ProductOrder);
            var entityDto = _mapper.Map<ProductDto>(updatedEntity);
            return Ok(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.UpdateProductOrder {ex.Message}");
            return Problem();
        }
    }

    [HttpDelete("{productId}")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> Delete(ProductIdModel model)
    {
        try
        {
            EntityDeleteResult deleteResult = await _productService.DeleteAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in ProductController.Delete {ex.Message}");
            return Problem();
        }
    }
}
