using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;

    public PropertyController(IPropertyService productPropertyService, IMapper mapper)
    {
        _propertyService = productPropertyService;
        _mapper = mapper;
    }

    // CREATE
    [HttpPost("Create")]
    public async Task<IActionResult> CreateProperty(Property entity)
    {
        var addedEntity = await _propertyService.AddAsync(entity);
        return CreatedAtRoute("GetProperty", new { id = addedEntity.Id }, _mapper.Map<PropertyDto>(addedEntity));
    }

    // READ
    [HttpGet("Get/{id}", Name = "GetProperty")]
    public async Task<IActionResult> GetProperty(Guid id)
    {
        var entity = await _propertyService.GetAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        var entityDto = _mapper.Map<PropertyDto>(entity);
        return Ok(entityDto);
    }


    [HttpGet("Get/Prop/Count/{categoryId}")]
    public async Task<IActionResult> GetPropertyCount(Guid categoryId)
    {

        var count = await _propertyService.GetEntryCountAsync(categoryId);

        return Ok(count);
    }


    [HttpGet("Get/properties/{categoryId}/{page}/{count}")]
    public async Task<IActionResult> GetCategoryProperties(Guid categoryId, int page, int count)
    {
        var entities = await _propertyService.GetAllAsNoTrackingAsync(categoryId, page, count);
        if (entities.Count == 0)
        {
            return Ok(new List<PropertyDto>());
        }

        var dtoEntities = _mapper.Map<IEnumerable<PropertyDto>>(entities);

        return Ok(dtoEntities);
    }
    [HttpGet("Get/properties/{categoryId}")]
    public async Task<IActionResult> GetCategoryProperties(Guid categoryId)
    {
        var entities = await _propertyService.GetAllAsNoTrackingAsync(categoryId);
        if (entities.Count == 0)
        {
            return NoContent();
        }

        var dtoEntities = _mapper.Map<IEnumerable<PropertyDto>>(entities);

        return Ok(dtoEntities);
    }
    [HttpGet("get/lastorder")]
    public async Task<IActionResult> GetLastItemOrder()
    {
        
        var order = await _propertyService.GetLastItemOrder();
        return Ok(order);
    }

    // UPDATE
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

    // DELETE
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteProperty(Guid id)
    {

        int deleted = await _propertyService.DeleteAsync(id);
        if (deleted > 0)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}
