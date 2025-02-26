using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Detail;
using TechStore.Api.Models.Product;

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

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDetailModel model, IValidator<CreateDetailModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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

    [HttpGet("get/{Id}", Name = "GetDetail")]
    public async Task<IActionResult> GetDetail([FromRoute] DetailIdModel model, IValidator<DetailIdModel> validator)
    {
        try
        {

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }

            var propertyValue = await _detailService.GetAsNoTrackingAsync(model.Id);
            return Ok(_mapper.Map<DetailMinimalDto>(propertyValue));
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetDetail {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("get/product/details/{Id}")]
    public async Task<IActionResult> GetProductDetails([FromRoute] ProductIdModel model, IValidator<ProductIdModel> validator)
    {
        try
        {

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var propertyValue = await _detailService.GetProductDetailsAsync(model.Id);
            if (propertyValue.Count == 0)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<List<DetailDto>>(propertyValue);
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in DetailController.GetProductDetails {ex.Message}");
            return Problem();
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateDetailModel model, IValidator<UpdateDetailModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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
    public async Task<IActionResult> Update([FromBody]List<UpdateDetailModel> model, IValidator<List<UpdateDetailModel>> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
            var ids = model.Select(m => m.Id).ToList();
            var existingDetails = await _detailService.GetByIdsAsync(ids);
            
            foreach (var entity in existingDetails)
            {
                var updateModel = model.Single(ue => ue.Id == entity.Id);
                _mapper.Map(updateModel, entity);
                entity.LastUpdate = DateTimeHelper.GetUtcNow();
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

    [HttpDelete("delete/{Id}")]
    public async Task<IActionResult> Delete([FromRoute] DetailIdModel model, IValidator<DetailIdModel> validator)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Validation failed.", errors = errorMessages });
            }
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

