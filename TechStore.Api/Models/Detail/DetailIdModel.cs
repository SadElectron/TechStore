using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Detail;

public class DetailIdModel
{
    [FromRoute(Name = "detailId")]
    public Guid Id { get; set; }
}
