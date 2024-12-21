using Domain.Common;

namespace Application.Extensions.ResultPattern;

public class BaseResult
{
    public bool IsSuccess { get; init; }
    public Error Error { get; init; }

    protected BaseResult(bool isSuccess, Error error)
    {
        Error = error;
        IsSuccess = isSuccess;
    }

    public static BaseResult Success() => new(true, Error.None());

    public static BaseResult Failure(Error error) => new(false, error);
}