using MarketStack.Common.ApiBase;
using MarketStack.Common.ResponseBase;
using MarketStack.Logic.Contracts.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller;

[Route("api/receipt")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IPriceAnalysisService _priceAnalysisService;

    public ReceiptController(IPriceAnalysisService priceAnalysisService)
    {
        _priceAnalysisService = priceAnalysisService;
    }

    [HttpGet("total-expenses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetTotalExpenses()
    {
        DataResponse<decimal> result = await _priceAnalysisService.GetTotalExpensesAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("total-tax-expenses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetTotalTaxExpenses()
    {
        DataResponse<decimal> result = await _priceAnalysisService.GetTotalTaxExpensesAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }
}