using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using System.Diagnostics;

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


    [HttpGet("get/product/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _productService.GetFullForCustomer(productId);
        if (product == null)
        {
            NoContent();
        }
        return Ok(product);
    }

    [HttpGet("get/product/count/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId = default)
    {
        int count = await _productService.GetProductCountAsync(categoryId);

        return Ok(count);
    }
    [HttpPost("get/product/filtered/count/{categoryId}")]
    public async Task<IActionResult> GetProductFilteredCount([FromBody] List<ProductFilterModel> filters, Guid categoryId = default)
    {
        int count = await _productService.GetFilteredCountAsync(filters, categoryId);

        return Ok(count);
    }

    [HttpGet("get/products/{categoryId}/{page}/{itemCount}")]
    public async Task<IActionResult> GetProducts(Guid categoryId, int page = 1, int itemCount = 10)
    {
        var products = await _productService.GetAllWithImagesAsync(page, itemCount, categoryId);
        if (products.Count == 0)
        {
            return NoContent();
        }
        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }
    [HttpGet("get/products")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var products = await _productService.GetAllAsNoTrackingAsync(q);
        
        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }

    [HttpGet("get/product/filters/{categoryId}")]
    public async Task<IActionResult> GetProductFilters([FromServices] IPropertyService propertyService, Guid categoryId = default)
    {
        var productFilters = await propertyService.GetProductFilters(categoryId);
        var filters = _mapper.Map<ICollection<CustomerProductFiltersDto>>(productFilters);
        return Ok(filters);
    }
    [HttpPost("get/products/filtered/{categoryId}/{page}/{itemCount}")]
    public async Task<IActionResult> GetProductsFiltered([FromBody] List<ProductFilterModel> filters, Guid categoryId = default, int page = 1, int itemCount = 10)
    {
        var products = await _productService.GetFilteredAsync(filters, categoryId, page, itemCount);
        var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
        return Ok(customerProductDtos);
    }

    [HttpPost("get/products/sorted/{categoryId}/{page}/{itemCount}")]
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

    [HttpGet("get/images/{productId}")]
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

    [HttpGet("get/categories")]
    public async Task<IActionResult> GetCategories([FromServices] ICategoryService categoryService)
    {
        var categories = await categoryService.GetAllAsync();
        var customerCategories = _mapper.Map<IEnumerable<CustomerCategoryDto>>(categories);
        return Ok(categories);
    }
}
