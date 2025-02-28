using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Detail;

public class DetailIdModel
{
    [FromRoute]
    public Guid Id { get; set; }
}
