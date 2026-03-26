// You created your own generic type (1 point)
public class OperationResult<T> where T : class
{
    public bool Success { get; }
    public T? Data { get; }
    public string Message { get; }

    private OperationResult(bool success, T? data, string message)
    {
        Success = success;
        Data = data;
        Message = message;
    }

    public static OperationResult<T> Ok(T data, string message = "Operation completed successfully.")
    {
        return new OperationResult<T>(true, data, message);
    }

    public static OperationResult<T> Fail(string message)
    {
        return new OperationResult<T>(false, null, message);
    }
}
