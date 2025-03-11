namespace Core.Results;

public record EntityCreateResult<T>(bool IsSuccessful, T? Entity, string Message = "");
