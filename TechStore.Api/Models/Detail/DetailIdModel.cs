using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Detail;

public class DetailIdModel : IValidationModel
{
    [FromRoute(Name = "detailId")]
    public Guid Id { get; set; }
}
