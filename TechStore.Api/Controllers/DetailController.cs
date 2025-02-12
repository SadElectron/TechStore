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
public class DetailController : ControllerBase
{
    private readonly IDetailService _detailService;
    private readonly IMapper _mapper;

    public DetailController(IDetailService detailService, IMapper mapper)
    {
        _detailService = detailService;
        _mapper = mapper;
    }

    // CREATE
    [HttpPost("create")]
    public async Task<IActionResult> Create(Detail detail)
    {
        var addedEntity = await _detailService.AddAsync(detail);
        if (addedEntity != null)
        {
            return CreatedAtRoute("GetDetail", new { id = addedEntity.Id },addedEntity);
        }
        return BadRequest();
    }

    // READ
    [HttpGet("get/{id}", Name = "GetDetail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var propertyValue = await _detailService.GetAsync(id);
        if (propertyValue == null)
        {
            return NotFound();
        }
        return Ok(propertyValue);
    }

    [HttpGet("get/product/details/{productId}")]
    public async Task<IActionResult> GetProductDetails(Guid productId)
    {
        var propertyValue = await _detailService.GetProductDetailsAsync(productId);
        var dtos = _mapper.Map<List<DetailDto>>(propertyValue);
        if (propertyValue == null)
        {
            return NotFound();
        }
        return Ok(dtos);
    }

    


    // UPDATE
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, Detail entityToUpdate)
    {
        var entity = await _detailService.GetAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        var updatedEntity = _detailService.UpdateAsync(entityToUpdate);

        return Ok(updatedEntity);
    }

    [HttpPut("update/product/details")]
    public async Task<IActionResult> Update(List<Detail> details)
    {
        var checkList = details.Select(d => _detailService.GetAsync(d.Id));
        var itemList = await Task.WhenAll(checkList);
        if (itemList.Length < details.Count || itemList.Length > details.Count)
        {
            return BadRequest();
        }
        var updatedDetails = await _detailService.UpdateDetailsAsync(details);

        return Ok(updatedDetails);
    }

    // DELETE
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        
        EntityDeleteResult deleteResult = await _detailService.DeleteAndReorderAsync(id);
        return deleteResult.IsSuccessful ? Ok() : NotFound();
    }
}

