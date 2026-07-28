namespace MarketStack.Common.ApiBase;

public class BaseResponse
{
    public bool Success { get; set; }

    public Exception? Exception { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public ErrorCodes ErrorCode { get; set; }

    public static BaseResponse CreateSuccessMessage(string title, string message)
    { 
        return new BaseResponse
        {
            Title = title,
            Message = message,
            Success = true
        };
    }

    public static BaseResponse CreateErrorMessage(string title, string message, ErrorCodes errorCode, Exception exception = null!)
    {
        return new BaseResponse
        {
            Title = title,
            Message = message,
            ErrorCode = errorCode,
            Success = false,
            Exception = exception
        };
    }
}