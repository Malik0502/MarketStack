using MarketStack.Common.ApiBase;
using MarketStack.Common.ResponseBase;
using MarketStack.Logic.Contracts.Dto;
using MarketStack.Logic.Contracts.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller;

[Route("api/expense")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseAnalysisService _expenseAnalysisService;

    public ExpenseController(IExpenseAnalysisService expenseAnalysisService)
    {
        _expenseAnalysisService = expenseAnalysisService;
    }

    [HttpGet("total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetTotalExpenses()
    {
        DataResponse<decimal> result = await _expenseAnalysisService.GetTotalExpensesAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("total-tax")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<decimal>>> GetTotalTaxExpenses()
    {
        DataResponse<decimal> result = await _expenseAnalysisService.GetTotalTaxExpensesAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("percentage-change")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<IDictionary<string, MonthlyExpenseSummary>>>> GetPercentageChangeSinceLastWeek()
    {
        DataResponse<decimal> result = await _expenseAnalysisService.GetPercentageChangeSinceLastWeekAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("tax-percentage-change")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<IDictionary<string, MonthlyExpenseSummary>>>> GetTaxPercentageChangeSinceLastWeek()
    {
        DataResponse<decimal> result = await _expenseAnalysisService.GetTaxPercentageChangeSinceLastWeekAsync();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<IDictionary<string, MonthlyExpenseSummary>>>> GetExpenseHistory()
    {
        DataResponse<IDictionary<string, MonthlyExpenseSummary>> result = await _expenseAnalysisService.GetExpenseHistory();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }

    [HttpGet("tax-history")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<DataResponse<IDictionary<string, MonthlyExpenseSummary>>>> GetTaxExpenseHistory()
    {
        DataResponse<IDictionary<string, MonthlyExpenseSummary>> result = await _expenseAnalysisService.GetTaxExpenseHistory();

        if (!result.Success)
        {
            var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();
            return StatusCode((int)httpStatus, result);
        }

        return Ok(result);
    }
}