using AutoMapper;
using Core.Entities.Concrete;
using Core.RequestModels;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Dtos;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;
using TechStore.Api.Models.Product;
using TechStore.Api.Models.Validation;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Models;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IProductService productService, IMapper mapper, ILogger<CustomerController> logger, IImageService imageService)
    {
        _productService = productService;
        _mapper = mapper;
        _logger = logger;
        _imageService = imageService;
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId, ProductIdValidator validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(productId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }

            var product = await _productService.GetFullForCustomer(productId);
            var dto = _mapper.Map<CustomerProductDto>(product);
            return Ok(dto);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CustomerController.GetProduct {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("product/count/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId, CategoryIdValidator validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(categoryId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            return Ok(await _productService.GetProductCountAsync(categoryId));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductCount: {ex.Message}");
            return Problem();
        }

    }

    [HttpPost("product/filtered/count/{categoryId}")]
    public async Task<IActionResult> GetProductFilteredCount(ProductFilteredCountModel model, IValidator<ProductFilteredCountModel> validator)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { message = "Validation failed.", errors = errorMessages });
        }
        int count = await _productService.GetFilteredCountAsync(model.Filters, model.CategoryId);

        return Ok(count);
    }

    [HttpGet("products/{categoryId}/{page}/{itemCount}")]
    public async Task<IActionResult> GetProducts(GetProductsModel model, IValidator<GetProductsModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var products = await _productService.GetAllWithImagesAsync(model.Page, model.ItemCount, model.CategoryId);
            var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
            return Ok(customerProductDtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProducts: {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("products")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var products = await _productService.GetAllAsNoTrackingAsync(q);

        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }

    [HttpGet("product/filters/{categoryId}")]
    public async Task<IActionResult> GetProductFilters([FromServices] IPropertyService propertyService, Guid categoryId = default)
    {
        var productFilters = await propertyService.GetProductFilters(categoryId);
        var filters = _mapper.Map<ICollection<CustomerProductFiltersDto>>(productFilters);
        return Ok(filters);
    }

    [HttpPost("products/filtered/{categoryId}/{page}/{itemCount}")]
    public async Task<IActionResult> GetProductsFiltered([FromBody] List<ProductFilterModel> filters, Guid categoryId = default, int page = 1, int itemCount = 10)
    {
        var products = await _productService.GetFilteredAsync(filters, categoryId, page, itemCount);
        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }

    [HttpPost("products/sorted/{categoryId}/{page}/{itemCount}")]
    public async Task<IActionResult> GetProductsFilteredAndSorted([FromBody] FilterAndSortModel filterAndSort, Guid categoryId = default, int page = 1, int itemCount = 10)
    {
        if (filterAndSort.sort == string.Empty || filterAndSort.sortValue == string.Empty)
        {
            return BadRequest();
        }
        var products = await _productService.GetFilteredAndSortedAsync(filterAndSort, categoryId, page, itemCount);
        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }

    [HttpGet("images/{productId}")]
    public async Task<IActionResult> GetImages(Guid productId)
    {
        var images = await _imageService.GetAllAsNoTrackingAsync(productId);
        if (images.Count == 0)
        {
            return NoContent();
        }
        var customerProductImages = _mapper.Map<IEnumerable<CustomerImageDto>>(images);
        return Ok(customerProductImages);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories([FromServices] ICategoryService categoryService)
    {
        var categories = await categoryService.GetAllAsync();
        var customerCategories = _mapper.Map<IEnumerable<CustomerCategoryDto>>(categories);
        return Ok(categories);
    }
}
