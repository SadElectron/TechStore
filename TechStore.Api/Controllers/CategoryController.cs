using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using TechStore.Api.Models.Category;

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


    // READ
    [HttpGet("Get/{Id}", Name = "GetCategory")]
    public async Task<IActionResult> GetCategory([FromRoute] CategoryIdModel model, IValidator<CategoryIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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

    [HttpGet("Get/{Page}/{Count}")]
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
                return Ok(Array.Empty<object>());
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

    [HttpGet("Get/full/{Page}/{Count}/{ProductPage}/{ProductCount}")]
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
                return Ok(Array.Empty<object>());
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

            _logger.LogWarning($"Error in CategoryController.GetCategoryCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("Get/ProductCount/{Id}")]
    public async Task<IActionResult> GetProductCount([FromRoute] CategoryIdModel model, IValidator<CategoryIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var count = await _categoryService.GetProductCountAsync(model.Id);
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in CategoryController.GetProductCount {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("Get/PropertyCount/{Id}")]
    public async Task<IActionResult> GetCategoryPropertyCount([FromRoute] CategoryIdModel model, IValidator<CategoryIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var count = await _categoryService.GetPropertyCount(model.Id);
            return Ok(count);
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in CategoryController.GetCategoryPropertyCount {ex.Message}");
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
            var addedEntity = await _categoryService.AddAsync(category);
            if (addedEntity != null)
            {
                return CreatedAtRoute("GetCategory", new { id = addedEntity.Id }, _mapper.Map<CategoryDto>(addedEntity));
            }
            return BadRequest();
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
            var entity = await _categoryService.GetAsync(model.Id);
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
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromBody]CategoryIdModel model, IValidator<CategoryIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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
