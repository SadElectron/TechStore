using AutoMapper;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Controllers;

[Authorize]
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
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> GetCategory(CategoryIdModel model)
    {
        try
        {
            var category = await _categoryService.GetAsNoTrackingAsync(model.Id);
            var categoryMinimalDto = _mapper.Map<CategoryMinimalDto>(category);
            return Ok(categoryMinimalDto);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategory {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("{page}/{itemCount}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetCategoriesModel>))]
    public async Task<IActionResult> GetCategories(GetCategoriesModel model)
    {
        try
        {
         
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
    [TypeFilter(typeof(ValidateByModelFilter<GetCategoriesFullModel>))]
    public async Task<IActionResult> GetCategoriesFull([FromRoute] GetCategoriesFullModel model)
    {
        try
        {
           
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
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> GetProductCount(CategoryIdModel model)
    {
        try
        {
            var count = await _categoryService.GetProductCountAsync(model.Id);
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.GetProductCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{categoryId}/property/count")]
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> GetCategoryPropertyCount(CategoryIdModel model)
    {
        try
        {
            var count = await _categoryService.GetPropertyCount(model.Id);
            return Ok(count);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategoryPropertyCount {ex.Message}");
            return Problem();
        }

    }

    [HttpGet("{categoryId}/properties/{page}/{count}")]
    [TypeFilter(typeof(ValidateByModelFilter<GetCategoryPropertiesModel>))]
    public async Task<IActionResult> GetCategoryProperties(GetCategoryPropertiesModel model, [FromServices]IPropertyService propertyService)
    {
        try
        {
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
    [TypeFilter(typeof(ValidateByModelFilter<CreateCategoryModel>))]
    public async Task<IActionResult> Create([FromBody] CreateCategoryModel model)
    {
        try
        {
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
    [HttpPut("update")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateCategoryModel>))]
    public async Task<IActionResult> Update(UpdateCategoryModel model)
    {
        try
        {
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
    [HttpPut("update/order")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateCategoryOrderModel>))]
    public async Task<IActionResult> UpdateOrder(UpdateCategoryOrderModel model)
    {
        try
        {
            var entity = await _categoryService.GetAsNoTrackingAsync(model.Id);
            var category = _mapper.Map(model, entity);
            var updatedEntity = await _categoryService.UpdateAsync(category!);
            return Ok(_mapper.Map<CategoryMinimalDto>(updatedEntity));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.UpdateOrder {ex.Message}");
            return Problem();
        }

    }

    // DELETE
    [HttpDelete("{categoryId}")]
    [TypeFilter(typeof(ValidateByModelFilter<CategoryIdModel>))]
    public async Task<IActionResult> Delete(CategoryIdModel model)
    {
        try
        {
            EntityDeleteResult deleteResult = await _categoryService.DeleteAndReorderAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.Delete {ex.Message}");
            return Problem();
        }

    }
}
