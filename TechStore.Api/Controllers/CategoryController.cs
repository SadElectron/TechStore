using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.Reflection.Metadata.Ecma335;
using TechStore.Api.Models;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryController> _logger;
    public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = logger;
    }

    // CREATE
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryModel entity)
    {
        var category = _mapper.Map<Category>(entity);
        var addedEntity = await _categoryService.AddAsync(category);
        if (addedEntity != null)
        {
            return CreatedAtRoute("GetCategory", new { id = addedEntity.Id }, _mapper.Map<CategoryDto>(addedEntity));
        }
        return BadRequest();
    }

    // READ
    [HttpGet("Get/{id}", Name = "GetCategory")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var productType = await _categoryService.GetAsync(id);
        if (productType == null)
        {
            return NotFound();
        }
        var categoryDto = _mapper.Map<CategoryDto>(productType);
        return Ok(categoryDto);
    }

    [HttpGet("Get/{page}/{count}")]
    public async Task<IActionResult> GetCategories(int page = 1, int count = 10)
    {
        try
        {
            var productTypes = await _categoryService.GetAllAsync(page, count);
            if (productTypes == null)
            {
                return NotFound();
            }
            var categoryDtos = _mapper.Map<List<Category>, IEnumerable<CategoryDto>>(productTypes);

            var taskList = categoryDtos.Select(async c => c.ProductCount = await _categoryService.GetProductCountAsync(c.Id));
            await Task.WhenAll(taskList);

            return Ok(categoryDtos);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound();
        }

    }

    [HttpGet("Get/full/{page}/{count}/{productPage}/{productCount}")]
    public async Task<IActionResult> GetCategoriesFull(int page = 1, int count = 10, int productPage = 1, int productCount = 10)
    {
        try
        {
            var categories = await _categoryService.GetFullAsync(page, count, productPage, productCount);
            if (categories == null)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<IEnumerable<CustomerCategoryDto>>(categories);
            return Ok(dtos);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound();
        }

    }

    [HttpGet("Get/Count")]
    public async Task<IActionResult> GetCategoryCount()
    {
        var productTypeCount = await _categoryService.GetEntryCountAsync();

        return Ok(productTypeCount);
    }

    [HttpGet("Get/ProductCount/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId)
    {
        var count = await _categoryService.GetProductCountAsync(categoryId);

        return Ok(count);
    }

    [HttpGet("Get/PropertyCount/{categoryId}")]
    public async Task<IActionResult> GetCategoryPropertyCount(Guid categoryId)
    {
        var count = await _categoryService.GetPropertyCount(categoryId);

        return Ok(count);
    }

    // UPDATE
    [HttpPut("Update")]
    public async Task<IActionResult> Update(Category entityToUpdate)
    {
        var entity = await _categoryService.GetAsync(entityToUpdate.Id);
        if (entity == null)
        {
            return NotFound();
        }
        var updatedEntity = await _categoryService.UpdateAndReorderAsync(entityToUpdate);

        return Ok(updatedEntity);
    }

    // DELETE
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        EntityDeleteResult deleteResult = await _categoryService.DeleteAndReorderAsync(id);
        return deleteResult.IsSuccessful ? Ok() : NotFound();

    }
}
