using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.Utils;
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
        try
        {
            var propertyValue = await _detailService.GetProductDetailsAsync(productId);

            if (propertyValue.Count == 0)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<List<DetailDto>>(propertyValue);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }




    // UPDATE
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateDetailModel updatedEntity)
    {
        try
        {
            var entityToUpdate = await _detailService.GetAsync(updatedEntity.Id);
            if (entityToUpdate == null)
            {
                return NotFound();
            }
            var detail = _mapper.Map(updatedEntity, entityToUpdate);
            var updateEntityResult = await _detailService.UpdateAsync(detail);
            if (updateEntityResult.IsSuccessful)
            {
                return Ok(_mapper.Map<DetailMinimalDto>(updateEntityResult.Entity));
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    [HttpPut("update/product/details")]
    public async Task<IActionResult> Update(List<UpdateDetailModel> updatedEntities)
    {
        try
        {
            var ids = updatedEntities.Select(ue => ue.Id).ToList();
            var existingDetails = await _detailService.GetByIdsAsync(ids);
            if (existingDetails.Count != updatedEntities.Count)
            {
                return BadRequest("Some entities were not found.");
            }
            foreach (var entity in existingDetails)
            {
                var updateModel = updatedEntities.Single(ue => ue.Id == entity.Id);
                _mapper.Map(updateModel, entity);
                entity.LastUpdate = DateTimeHelper.GetUtcNow();
            }

            var updatedDetails = await _detailService.UpdateDetailsAsync(existingDetails);

            return Ok(_mapper.Map<List<DetailMinimalDto>>(updatedDetails));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Problem();
        }
    }

    // DELETE
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            EntityDeleteResult deleteResult = await _detailService.DeleteAndReorderAsync(id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, ex.Message);
            return Problem();
        }
        
    }
}

