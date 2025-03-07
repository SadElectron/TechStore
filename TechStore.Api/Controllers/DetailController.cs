using AutoMapper;
using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Detail;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/details")]
[Authorize("Admin")]
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

    [HttpPost("create")]
    [TypeFilter(typeof(ValidateByModelFilter<CreateDetailModel>))]
    public async Task<IActionResult> Create(CreateDetailModel model)
    {
        try
        {
            var detail = _mapper.Map<Detail>(model);
            EntityCreateResult<Detail> result = await _detailService.AddAsync(detail);
            return result.IsSuccessful ?
                CreatedAtRoute("GetDetail", new { id = result.Entity!.Id }, _mapper.Map<DetailMinimalDto>(result.Entity)) :
                BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.Create {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("{Id}", Name = "GetDetail")]
    [TypeFilter(typeof(ValidateByModelFilter<DetailIdModel>))]
    public async Task<IActionResult> GetDetail(DetailIdModel model)
    {
        try
        {
            var propertyValue = await _detailService.GetAsNoTrackingAsync(model.Id);
            return Ok(_mapper.Map<DetailMinimalDto>(propertyValue));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetDetail {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("update")]
    [TypeFilter(typeof(ValidateByModelFilter<UpdateDetailModel>))]
    public async Task<IActionResult> Update(UpdateDetailModel model)
    {
        try
        {
            var entityToUpdate = await _detailService.GetAsync(model.Id);
            var detail = _mapper.Map(model, entityToUpdate);
            EntityUpdateResult<Detail> result = await _detailService.UpdateAsync(detail!);
            if (result.IsSuccessful)
            {
                return Ok(_mapper.Map<DetailMinimalDto>(result.Entity));
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetProductDetails {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("update/product/details")]
    [TypeFilter(typeof(ValidateByModelFilter<List<UpdateDetailModel>>))]
    public async Task<IActionResult> Update([FromBody]List<UpdateDetailModel> model)
    {
        try
        {
            var ids = model.Select(m => m.Id).ToList();
            var existingDetails = await _detailService.GetByIdsAsync(ids);
            var timeNowUtc = DateTimeHelper.GetUtcNow();    
            foreach (var entity in existingDetails)
            {
                var updateModel = model.Single(ue => ue.Id == entity.Id);
                _mapper.Map(updateModel, entity);
                entity.LastUpdate = timeNowUtc;
            }

            var updatedDetails = await _detailService.UpdateDetailsAsync(existingDetails);

            return Ok(_mapper.Map<List<DetailMinimalDto>>(updatedDetails));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetProductDetails {ex.Message}");
            return Problem();
        }
    }

    [HttpDelete("{detailId}")]
    [TypeFilter(typeof(ValidateByModelFilter<DetailIdModel>))]
    public async Task<IActionResult> Delete(DetailIdModel model)
    {
        try
        {
            EntityDeleteResult deleteResult = await _detailService.DeleteAndReorderAsync(model.Id);
            return deleteResult.IsSuccessful ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.Delete {ex.Message}");
            return Problem();
        }
    }
}

