using System.Linq;

namespace TechStore.Api.Validation.Utils;

public static class ValidationUtils
{
    public static bool ContainsSuspiciousCharacters(string input)
    {
        string[] forbiddenPatterns = { "--", ";", "'", "<script>" };
        return forbiddenPatterns.Any(input.Contains);
    }
}
