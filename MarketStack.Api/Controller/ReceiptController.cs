using MarketStack.Common.ApiBase;
using MarketStack.Common.ResponseBase;
using MarketStack.Logic.Contracts.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller;

[Route("api/receipt")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IReceiptAnalysisService _receiptAnalysisService;

    public ReceiptController(IReceiptAnalysisService receiptAnalysisService)
    {
        _receiptAnalysisService = receiptAnalysisService;
    }

    [HttpGet("total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<int>>> GetTotalPurchases()
    {
        DataResponse<int> result = await _receiptAnalysisService.GetTotalPurchases();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("average-purchase-value")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetAveragePurchaseValue()
    {
        DataResponse<decimal> result = await _receiptAnalysisService.GetAveragePurchaseValue();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("average-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetAverageItemsPerPurchase()
    {
        DataResponse<decimal> result = await _receiptAnalysisService.GetAverageItemsPerPurchase();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("discount-share")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetDiscountedItemShare()
    {
        DataResponse<decimal> result = await _receiptAnalysisService.GetDiscountedItemShare();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }
}