using Microsoft.AspNetCore.Http;

namespace TechStore.Api.Dtos;

public class ImagesDto
{
    public int Order { get; set; }
    public IFormFile Image { get; set; }
}
