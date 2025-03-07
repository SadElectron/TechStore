namespace TechStore.Api.Models.Auth;

public class TokenRefreshModel
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
