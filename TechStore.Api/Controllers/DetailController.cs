using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Models;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class DetailController : ControllerBase
{
    private readonly IDetailService _detailService;
    private readonly IMapper _mapper;
    private readonly ILogger<DetailController> _logger;

    public DetailController(IDetailService detailService, IMapper mapper, ILogger<DetailController> logger)
    {
        _detailService = detailService;
        _mapper = mapper;
        _logger = logger;
    }

    // CREATE
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDetailModel model)
    {
        try
        {
            var detail = _mapper.Map<Detail>(model);
            EntityAddResult<Detail> entityAddResult = await _detailService.AddAsync(detail);
            if (entityAddResult.IsSuccessful)
            {
                return CreatedAtRoute("GetDetail", new { id = entityAddResult.Entity!.Id }, _mapper.Map<DetailMinimalDto>(entityAddResult.Entity));
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
    [HttpGet("get/{id}", Name = "GetDetail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        try
        {
            var propertyValue = await _detailService.GetAsNoTrackingAsync(id);
            if (propertyValue == null)
            {
                return NotFound();
            }
            return Ok(propertyValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
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

