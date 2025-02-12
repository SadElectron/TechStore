using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Services.Abstract;
using Services.Concrete;


namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[Authorize(Policy = "Admin")]
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

    // CREATE
    [HttpPost("Create")]
    public async Task<IActionResult> CreateProduct([FromForm] Product entity, [FromForm] List<IFormFile> images)
    {

        Product addedEntity = await _productService.AddAsync(entity, images);
        var addedEntityDto = _mapper.Map<ProductDto>(addedEntity);
        return Ok(addedEntityDto);
    }

    // READ
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var entity = await _productService.GetAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<ProductDto>(entity);
        return Ok(dto);
    }

    [HttpGet("Get/{page}/{count}")]
    public async Task<IActionResult> GetProducts(int page, int count)
    {
        var entities = await _productService.GetAllAsync(page, count);
        if (entities == null)
        {
            return Ok(new());
        }
        var dtos = _mapper.Map<ICollection<ProductDto>>(entities);


        return Ok(dtos);
    }

    [HttpGet("Get/Count")]
    public async Task<IActionResult> GetEntryCount()
    {
        var entityCount = await _productService.GetEntryCountAsync();
        return Ok(entityCount);
    }
    [HttpGet("Get/Count/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId)
    {
        var productCount = await _productService.GetProductCountAsync(categoryId);
        return Ok(productCount);
    }
    [HttpGet("Reorder")]
    public async Task<IActionResult> Reorder()
    {
        await _productService.ReorderDb();
        return Ok();
    }
    [HttpGet("Reorder/{categoryId}")]
    public async Task<IActionResult> ReorderCategory(Guid categoryId)
    {
        await _productService.ReorderCategoryProducts(categoryId);
        return Ok();
    }


    // UPDATE
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProduct([FromBody]Product entityToUpdate)
    {
        
        var updatedEntity = await _productService.UpdateAndReorderAsync(entityToUpdate);
        if (updatedEntity.ProductName == string.Empty)
        {
            return BadRequest();
        }
        var entityDto = _mapper.Map<ProductDto>(updatedEntity);
        return Ok(entityDto);
    }


    // DELETE
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {

        EntityDeleteResult deleteResult = await _productService.DeleteAndReorderAsync(id);
        return deleteResult.IsSuccessful ? Ok() : NotFound();
    }
}
