namespace Core.Results;

public record LoginResult
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; } = new();
}