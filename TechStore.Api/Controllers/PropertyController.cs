using AutoMapper;
using Core.Entities.Concrete;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
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
    public async Task<IActionResult> UpdateProperty(UpdatePropertyModel model, IValidator<UpdatePropertyModel> validator)
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
            var property = _mapper.Map(model, entity);
            var addedEntity = await _propertyService.UpdateAsync(property!);
            var dto = _mapper.Map<PropertyDto>(addedEntity);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in PropertyController.UpdateProperty {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("{propertyId}/proporder")]
    public async Task<IActionResult> UpdateOrder(UpdatePropOrderModel model, IValidator<UpdatePropOrderModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var result = await _propertyService.UpdatePropOrderAsync(model.Id, model.PropOrder);
            var dto = _mapper.Map<PropertyDto>(result);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in PropertyController.UpdateOrder {ex.Message}");
            return Problem();
        }

    }

    [HttpDelete("{propertyId}")]
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
