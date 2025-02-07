namespace Core.Dtos;

public class CustomerProductFiltersDto
{
    public string PropName { get; set; } = string.Empty;
    public List<FilterValueDto> Values { get; set; } = new List<FilterValueDto>();
}
