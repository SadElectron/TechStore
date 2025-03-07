namespace Core.Results;

public class RegisterUserResult
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresIn { get; set; }
    public bool IsSuccessful { get; set; }
    public List<string> Errors { get; set; } = new();
}






