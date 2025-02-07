using Microsoft.AspNetCore.Http;

namespace Core.Dtos;

public class ImagesDto
{
    public int Order { get; set; }
    public IFormFile Image { get; set; }
}
