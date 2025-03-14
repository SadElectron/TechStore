using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;
using TechStore.Api.Models.Product;

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

    [HttpGet("products/{productId}")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> GetProduct(ProductIdModel model)
    {
        try
        {
            var product = await _productService.GetFullForCustomer(model.Id);
            var dto = _mapper.Map<CustomerProductDto>(product);
            return Ok(dto);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CustomerController.GetProduct {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("categories/{categoryId}/products/count/")]
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> GetProductCount(CategoryIdModel model)
    {
        try
        {
            return Ok(await _productService.GetProductCountAsync(model.Id));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductCount: {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("categories/full/{Page}/{Count}/{ProductPage}/{ProductCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetCategoriesFullModel>))]
    public async Task<IActionResult> GetCategoriesFull([FromRoute] GetCategoriesFullModel model, ICategoryService categoryService)
    {
        try
        {

            var categories = await categoryService.GetFullAsync(model.Page, model.Count, model.ProductPage, model.ProductCount);
            if (categories.Count == 0)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<IEnumerable<CustomerCategoryDto>>(categories);
            return Ok(dtos);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategoriesFull {ex.Message}");
            return Problem();
        }

    }

    [HttpPost("categories/{categoryId}/products/filtered/count")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductFilteredCountModel>))]
    public async Task<IActionResult> GetProductFilteredCount(ProductFilteredCountModel model)
    {
        int count = await _productService.GetFilteredCountAsync(model.Filters, model.CategoryId);
        return Ok(count);
    }

    [HttpGet("categories/{categoryId}/products/{page}/{itemCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetProductsModel>))]
    public async Task<IActionResult> GetProducts(GetProductsModel model)
    {
        try
        {
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
    [TypeFilter(typeof(ValidateByModelFilter<SearchModel>))]
    public async Task<IActionResult> Search(SearchModel model)
    {
        try
        {
            var products = await _productService.GetAllAsNoTrackingAsync(model.Query);
            var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
            return Ok(customerProductDtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.Search: {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("categories/{categoryId}/filters")]
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> GetProductFilters(CategoryIdModel model, [FromServices] IPropertyService propertyService)
    {
        try
        {
            var productFilters = await propertyService.GetProductFilters(model.Id);
            var filters = _mapper.Map<ICollection<CustomerProductFiltersDto>>(productFilters);
            return Ok(filters);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductFilters: {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("categories/{categoryId}/products/filtered/{page}/{itemCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetProductsFilteredModel>))]
    public async Task<IActionResult> GetProductsFiltered(GetProductsFilteredModel model)
    {
        try
        {
            var products = await _productService.GetFilteredAsync(model.Filters, model.CategoryId, model.Page, model.ItemCount);
            var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
            return Ok(customerProductDtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductFilters: {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("categories/{categoryId}/products/sorted/{page}/{itemCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetProductsFilteredAndSortedModel>))]
    public async Task<IActionResult> GetProductsFilteredAndSorted(GetProductsFilteredAndSortedModel model)
    {
        try
        {
            var products = await _productService.GetFilteredAndSortedAsync(model.FilterAndSort, model.CategoryId, model.Page, model.ItemCount);
            var customerProductDtos = _mapper.Map<ICollection<CustomerProductDto>>(products);
            return Ok(customerProductDtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductFilters: {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("products/{productId}/images")]
    [TypeFilter(typeof(ValidateByModelFilter<ProductIdModel>))]
    public async Task<IActionResult> GetImages(ProductIdModel model)
    {
        try
        {
            var images = await _imageService.GetAllAsNoTrackingAsync(model.Id);
            if (images.Count == 0)
            {
                return NoContent();
            }
            var customerProductImages = _mapper.Map<IEnumerable<CustomerImageDto>>(images);
            return Ok(customerProductImages);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductFilters: {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("categories/{page}/{itemCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetCategoriesModel>))]
    public async Task<IActionResult> GetCategories(GetCategoriesModel model, [FromServices] ICategoryService categoryService)
    {
        try
        {
            var categories = await categoryService.GetAllAsNoTrackingAsync(model.Page, model.Count);
            var customerCategories = _mapper.Map<IEnumerable<CustomerCategoryDto>>(categories);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CustomerController.GetProductFilters: {ex.Message}");
            return Problem();
        }
    }
}
