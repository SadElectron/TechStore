using AutoMapper;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/categories")]
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


    // READ
    [HttpGet("{categoryId}", Name = "GetCategory")]
    public async Task<IActionResult> GetCategory(Guid categoryId, CategoryIdValidator validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(categoryId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var category = await _categoryService.GetAsNoTrackingAsync(categoryId);
            var categoryMinimalDto = _mapper.Map<CategoryMinimalDto>(category);
            return Ok(categoryMinimalDto);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategory {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("{Page}/{Count}")]
    public async Task<IActionResult> GetCategories([FromRoute] GetCategoriesModel model, IValidator<GetCategoriesModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }

            var categories = await _categoryService.GetAllAsync(model.Page, model.Count);
            if (categories.Count == 0)
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

            _logger.LogWarning($"Error in CategoryController.GetCategories {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("full/{Page}/{Count}/{ProductPage}/{ProductCount}")]
    public async Task<IActionResult> GetCategoriesFull([FromRoute] GetCategoriesFullModel model, IValidator<GetCategoriesFullModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var categories = await _categoryService.GetFullAsync(model.Page, model.Count, model.ProductPage, model.ProductCount);
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

    [HttpGet("count")]
    public async Task<IActionResult> GetCategoryCount()
    {
        try
        {
            var categoryCount = await _categoryService.GetEntryCountAsync();
            return Ok(categoryCount);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategoryCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{categoryId}/products/count")]
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
            var count = await _categoryService.GetProductCountAsync(categoryId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.GetProductCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{categoryId}/property/count")]
    public async Task<IActionResult> GetCategoryPropertyCount(Guid categoryId, CategoryIdValidator validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(categoryId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var count = await _categoryService.GetPropertyCount(categoryId);
            return Ok(count);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategoryPropertyCount {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("{categoryId}/properties/{page}/{count}")]
    public async Task<IActionResult> GetCategoryProperties(GetCategoryPropertiesModel model, IValidator<GetCategoryPropertiesModel> validator, [FromServices]IPropertyService propertyService)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var entities = await propertyService.GetAllAsNoTrackingAsync(model.CategoryId, model.Page, model.Count);
            var dtoEntities = _mapper.Map<IEnumerable<PropertyDto>>(entities);
            return Ok(dtoEntities);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.GetCategoryProperties {ex.Message}");
            return Problem();
        }
        
    }
    // CREATE
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryModel model, IValidator<CreateCategoryModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var category = _mapper.Map<Category>(model);
            EntityCreateResult<Category> result = await _categoryService.AddAsync(category);
            return result.IsSuccessful ?
                CreatedAtRoute("GetCategory", new { categoryId = result.Entity!.Id }, _mapper.Map<CategoryDto>(result.Entity)) :
                BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.Create {ex.Message}");
            return Problem();

        }

    }
    // UPDATE
    [HttpPut("Update")]
    public async Task<IActionResult> Update(UpdateCategoryModel model, IValidator<UpdateCategoryModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var entity = await _categoryService.GetAsNoTrackingAsync(model.Id);
            var category = _mapper.Map(model, entity);
            var updatedEntity = await _categoryService.UpdateAsync(category!);
            return Ok(_mapper.Map<CategoryMinimalDto>(updatedEntity));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.Update {ex.Message}");
            return Problem();
        }

    }

    // DELETE
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(Guid categoryId, CategoryIdValidator validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(categoryId);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            EntityDeleteResult deleteResult = await _categoryService.DeleteAndReorderAsync(categoryId);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.Delete {ex.Message}");
            return Problem();
        }

    }
}
