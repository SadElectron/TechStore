using AutoMapper;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Concrete;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Property;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/properties")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(IPropertyService productPropertyService, IMapper mapper, ILogger<PropertyController> logger)
    {
        _propertyService = productPropertyService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{propertyId}", Name = "GetProperty")]
    public async Task<IActionResult> GetProperty(PropertyIdModel model, IValidator<PropertyIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }

            var entity = await _propertyService.GetAsNoTrackingAsync(model.Id);
            var entityDto = _mapper.Map<PropertyDto>(entity);
            return Ok(entityDto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in PropertyController.GetProperty {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyModel model, IValidator<CreatePropertyModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var property = _mapper.Map<Property>(model);
            EntityCreateResult<Property> result = await _propertyService.AddAsync(property);

            return CreatedAtRoute("GetProperty", new { propertyId = result.Entity!.Id }, _mapper.Map<PropertyDto>(result.Entity));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in PropertyController.CreateProperty {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProperty(Property property)
    {
        var addedEntity = await _propertyService.UpdateAsync(property);

        if (addedEntity == null)
        {
            return NoContent();
        }

        return Ok(addedEntity);
    }

    [HttpPut("Update/All")]
    public async Task<IActionResult> UpdateProperties([FromForm] List<Property> properties)
    {
        await _propertyService.UpdateAllAsync(properties);

        var count = await _propertyService.GetEntryCountAsync(properties[0].CategoryId);
        var updatedEntities = await _propertyService.GetAllAsync(properties[0].CategoryId, 1, count);
        var updatedEntitiesMapped = _mapper.Map<ICollection<PropertyDto>>(updatedEntities);
        return Ok(updatedEntitiesMapped);
    }

    [HttpDelete("Delete/{propertyId}")]
    public async Task<IActionResult> Delete(PropertyIdModel model, IValidator<PropertyIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            EntityDeleteResult deleteResult = await _propertyService.DeleteAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in PropertyController.Delete {ex.Message}");
            return Problem();
        }
    }
}
