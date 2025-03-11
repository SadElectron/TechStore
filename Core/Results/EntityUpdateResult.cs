namespace Core.Results;
public record EntityUpdateResult<T>(bool IsSuccessful, T? Entity, string Message = "");
