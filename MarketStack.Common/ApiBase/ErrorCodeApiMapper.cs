using System.Net;

namespace MarketStack.Common.ApiBase;

public static class ErrorCodeApiMapper
{
    public static ErrorCodes MapHttpStatusCodeToErrorCode(this HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.OK => ErrorCodes.None,
            HttpStatusCode.Unauthorized => ErrorCodes.Unauthorized,
            HttpStatusCode.BadRequest => ErrorCodes.Validation,
            HttpStatusCode.NotFound => ErrorCodes.NotFound,
            HttpStatusCode.ServiceUnavailable => ErrorCodes.ExternalService,
            HttpStatusCode.BadGateway => ErrorCodes.ParseError,
            HttpStatusCode.InternalServerError => ErrorCodes.InternalError,
            
            _ => ErrorCodes.None
        };
    }

    public static HttpStatusCode MapErrorCodeToHttpStatusCode(this ErrorCodes errorCode)
    {
        return errorCode switch
        {
            ErrorCodes.None => HttpStatusCode.OK,
            ErrorCodes.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorCodes.Validation => HttpStatusCode.BadRequest,
            ErrorCodes.NotFound => HttpStatusCode.NotFound,
            ErrorCodes.ExternalService => HttpStatusCode.ServiceUnavailable,
            ErrorCodes.ParseError => HttpStatusCode.BadGateway,
            ErrorCodes.InternalError => HttpStatusCode.InternalServerError,
            
            _ => HttpStatusCode.OK
        };
    }
}