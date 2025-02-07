namespace Core.RequestModels;

public class ProductFilterModel
{
    public string FilterKey { get; set; } = string.Empty;
    public List<string> FilterValues { get; set; } = new List<string>();
}
