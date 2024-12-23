using Domain.Common;

namespace Application.Extensions.Responses.ApiResponse;

public class ApiResponse<T>
{
    public bool IsSuccess { get; init; }
    public Error Error { get; init; }
    public T? Data { get; init; }

    private ApiResponse(bool isSuccess, Error error, T? data)
    {
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    public static ApiResponse<T> Success(T? data) => new(true, Error.None(), data);

    public static ApiResponse<T> Fail(Error error) => new(false, error, default);
}