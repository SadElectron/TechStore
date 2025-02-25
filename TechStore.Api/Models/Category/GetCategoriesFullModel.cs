namespace TechStore.Api.Models.Category;

public class GetCategoriesFullModel
{
    public int Page { get; set; }
    public int Count { get; set; }
    public int ProductPage { get; set; }
    public int ProductCount { get; set; }
}
