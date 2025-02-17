using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> Create([FromBody] CreateCategoryModel model)
    {
        try
        {
            var category = _mapper.Map<Category>(model);
            var addedEntity = await _categoryService.AddAsync(category);
            if (addedEntity != null)
            {
                return CreatedAtRoute("GetCategory", new { id = addedEntity.Id }, _mapper.Map<CategoryDto>(addedEntity));
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }


    }

    // READ
    [HttpGet("Get/{id}", Name = "GetCategory")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        try
        {
            var category = await _categoryService.GetAsNoTrackingAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryMinimalDto = _mapper.Map<CategoryMinimalDto>(category);
            return Ok(categoryMinimalDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }

    }

    [HttpGet("Get/{page}/{count}")]
    public async Task<IActionResult> GetCategories(int page = 1, int count = 10)
    {
        try
        {
            var categories = await _categoryService.GetAllAsync(page, count);
            if (categories == null)
            {
                return NotFound();
            }
            var categoryDtos = _mapper.Map<List<Category>, IEnumerable<CategoryDto>>(categories);

            var taskList = categoryDtos.Select(async c => c.ProductCount = await _categoryService.GetProductCountAsync(c.Id));
            await Task.WhenAll(taskList);
            return Ok(categoryDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem(ex.Message);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    [HttpGet("Get/Count")]
    public async Task<IActionResult> GetCategoryCount()
    {
        try
        {
            var categoryCount = await _categoryService.GetEntryCountAsync();

            return Ok(categoryCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    [HttpGet("Get/ProductCount/{categoryId}")]
    public async Task<IActionResult> GetProductCount(Guid categoryId)
    {
        try
        {
            var count = await _categoryService.GetProductCountAsync(categoryId);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    [HttpGet("Get/PropertyCount/{categoryId}")]
    public async Task<IActionResult> GetCategoryPropertyCount(Guid categoryId)
    {
        try
        {
            var count = await _categoryService.GetPropertyCount(categoryId);

            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    // UPDATE
    [HttpPut("Update")]
    public async Task<IActionResult> Update(UpdateCategoryModel entityToUpdate)
    {
        try
        {
            var entity = await _categoryService.GetAsync(entityToUpdate.Id);

            if (entity == null)
            {
                return NotFound();
            }
            var category = _mapper.Map(entityToUpdate, entity);
            var updatedEntity = await _categoryService.UpdateAndReorderAsync(category);

            return Ok(_mapper.Map<CategoryMinimalDto>(updatedEntity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    // DELETE
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            EntityDeleteResult deleteResult = await _categoryService.DeleteAndReorderAsync(id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }

    }
}
