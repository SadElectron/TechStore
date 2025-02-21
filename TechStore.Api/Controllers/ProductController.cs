using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models;


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
    public async Task<IActionResult> GetProducts(int page, int count)
    {
        try
        {
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
    public async Task<IActionResult> GetProductCount(Guid categoryId)
    {
        try
        {
            var productCount = await _productService.GetProductCountAsync(categoryId);
            return Ok(productCount);
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }
        
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProduct(CreateProductModel model, [FromServices]ICategoryService categoryService)
    {
        var errors = new List<string>();

        // Validate CategoryId
        if (model.CategoryId == Guid.Empty)
            errors.Add("CategoryId is required and cannot be empty.");
        if (!await categoryService.ExistsAsync(model.CategoryId))
            errors.Add("Category not found.");

        // Validate ProductName
        if (string.IsNullOrWhiteSpace(model.ProductName) || model.ProductName.Length < 2)
            errors.Add("ProductName is required and must be at least 2 characters long.");

        // Validate Stock
        if (model.Stock < 1)
            errors.Add("Stock must be at least 1.");

        // Validate Price
        if (model.Price <= 0)
            errors.Add("Price must be greater than zero.");

        if (errors.Any())
            return BadRequest(new { message = "Validation failed.", errors });

        var entity = _mapper.Map<Product>(model);

        var createEntityResult = await _productService.AddAsync(entity);
        if(!createEntityResult.IsSuccessful)
        {
            return BadRequest(new { message = createEntityResult.Message});
        }
        var addedEntityDto = _mapper.Map<ProductDto>(createEntityResult.Entity);
        return Ok(addedEntityDto);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product entityToUpdate)
    {

        var updatedEntity = await _productService.UpdateAndReorderAsync(entityToUpdate);
        if (updatedEntity.ProductName == string.Empty)
        {
            return BadRequest();
        }
        var entityDto = _mapper.Map<ProductDto>(updatedEntity);
        return Ok(entityDto);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {

        EntityDeleteResult deleteResult = await _productService.DeleteAndReorderAsync(id);
        return deleteResult.IsSuccessful ? Ok() : NotFound();
    }
}
