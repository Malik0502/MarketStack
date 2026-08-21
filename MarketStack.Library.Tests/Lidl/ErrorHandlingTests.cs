using System.Net;
using MarketStack.Common.ApiBase;
using MarketStack.Common.ErrorHandling;
using MarketStack.Common.ResponseBase;
using MarketStack.Library.Contracts.Helper;
using MarketStack.Library.Receipt.Lidl;
using NSubstitute;

namespace MarketStack.Library.Tests.Lidl;

public class ErrorHandlingTests
{
    [Fact]
    public async Task GetAuthTokenAsync_ShouldReturnUnauthorizedError_GivenUnauthorizedApiCall()
    {
        // Arrange
        var fetchBaseResult = new FetchBase()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized),
            Json = null
        };

        var fetchClient = Substitute.For<IFetchClient>();

        fetchClient.FetchJsonAsync(Arg.Any<string>(), Arg.Any<HttpClient>()).Returns(fetchBaseResult);

        var receiptClient = new LidlReceiptClient(fetchClient);

        var expectedResult = DataResponse<string>.CreateErrorResponse("Failed to retrieve authentication token.", 
        "There was an error while fetching the authentication token.", fetchBaseResult.HttpResponseMessage.StatusCode.MapHttpStatusCodeToErrorCode());
        
        // Act
        var result = await receiptClient.GetAuthTokenAsync();

        // Assert

        Assert.Null(result.Data);
        Assert.False(result.Success);
        Assert.Equal(expectedResult.Title, result.Title);
        Assert.Equal(expectedResult.Message, result.Message);
        Assert.Equal(expectedResult.ErrorCode, result.ErrorCode);
    }

    [Fact]
    public async Task GetAuthTokenAsync_ShouldReturnParsingError_GivenEmptyJson()
    {
        // Arrange
        var fetchBaseResult = new FetchBase()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway),
            Json = "{\"token\":\"\"}"
        };

        var fetchClient = Substitute.For<IFetchClient>();

        fetchClient.FetchJsonAsync(Arg.Any<string>(), Arg.Any<HttpClient>()).Returns(fetchBaseResult);

        var receiptClient = new LidlReceiptClient(fetchClient);

        var expectedResult = DataResponse<string>.CreateErrorResponse("Failed to retrieve authentication token.",
            "There was an error while deserializing the authentication token.", ErrorCodes.ParseError);

        // Act
        var result = await receiptClient.GetAuthTokenAsync();

        // Assert

        Assert.Null(result.Data);
        Assert.False(result.Success);
        Assert.Equal(expectedResult.Title, result.Title);
        Assert.Equal(expectedResult.Message, result.Message);
        Assert.Equal(expectedResult.ErrorCode, result.ErrorCode);
    }

    [Fact]
    public async Task GetAuthTokenAsync_ShouldReturnException_GivenWrongJsonFormat()
    {
        // Arrange
        var fetchBaseResult = new FetchBase()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway),
            Json = "Not right"
        };

        var fetchClient = Substitute.For<IFetchClient>();

        fetchClient.FetchJsonAsync(Arg.Any<string>(), Arg.Any<HttpClient>()).Returns(fetchBaseResult);

        var receiptClient = new LidlReceiptClient(fetchClient);

        var expectedResult = DataResponse<string>.CreateErrorResponse("Exception occurred",
            string.Empty, ErrorCodes.InternalError);

        // Act
        var result = await receiptClient.GetAuthTokenAsync();

        // Assert

        Assert.Null(result.Data);
        Assert.False(result.Success);
        Assert.Equal(expectedResult.Title, result.Title);
        Assert.Equal(expectedResult.ErrorCode, result.ErrorCode);
    }

    [Fact]
    public async Task GetAuthTokenAsync_ShouldReturnSuccess_GivenValidJson()
    {
        // Arrange
        var fetchBaseResult = new FetchBase()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway),
            Json = "{\"token\":\"AuthToken\"}"
        };

        var fetchClient = Substitute.For<IFetchClient>();

        fetchClient.FetchJsonAsync(Arg.Any<string>(), Arg.Any<HttpClient>()).Returns(fetchBaseResult);

        var receiptClient = new LidlReceiptClient(fetchClient);

        var expectedResult = DataResponse<string>.CreateSuccessResponse("AuthToken", 
            "Success",
            "Authentication token retrieved successfully.");

        // Act
        var result = await receiptClient.GetAuthTokenAsync();

        // Assert
        Assert.Equal(expectedResult.Data, result.Data);
        Assert.True(result.Success);
        Assert.Equal(expectedResult.Title, result.Title);
        Assert.Equal(expectedResult.Message, result.Message);
    }
}