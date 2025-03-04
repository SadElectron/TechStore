using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Validation;

public class ValidationResultModel
{
    public record ResponseModel(string Message,  IEnumerable<string> Errors);
    public bool IsValid { get; set; }
    public ResponseModel Response { get; set; } = new ResponseModel("Validation failed.", new List<string>());
}
