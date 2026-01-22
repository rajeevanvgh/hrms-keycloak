namespace Hrms.EmployeeService.Application.Common;

public sealed class Result<T>
{
    public required bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }

    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data,
        Error = null
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Data = default,
        Error = error
    };
}