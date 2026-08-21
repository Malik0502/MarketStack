using MarketStack.Common.ErrorHandling;

namespace MarketStack.Common.ResponseBase;

public class DataResponse<T> : BaseResponse where T : class
{
    public T? Data { get; set; }

    public static DataResponse<T> CreateSuccessResponse(T data, string title, string message)
    {
        return new DataResponse<T>
        {
            Data = data,
            Title = title,
            Message = message,
            Success = true
        };
    }

    public static DataResponse<T> CreateErrorResponse(string title, string message, ErrorCodes errorCode, Exception exception = null!)
    {
        return new DataResponse<T>
        {
            Title = title,
            Message = message,
            ErrorCode = errorCode,
            Success = false,
            Exception = exception
        };
    }
}