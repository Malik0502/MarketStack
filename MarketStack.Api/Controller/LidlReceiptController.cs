using MarketStack.Common.ApiBase;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts;
using MarketStack.Logic.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LidlReceiptController : ControllerBase
    {
        private readonly IReceiptInformationManager _receiptInformationManager;
        private readonly IReceiptRepository _repository;

        public LidlReceiptController(IReceiptInformationManager receiptInformationManager, IReceiptRepository repository)
        {
            _receiptInformationManager = receiptInformationManager;
            _repository = repository;
        }

        // Date Time Error while saving to db
        [HttpPost("ToDb")]
        public async Task<ActionResult> FillDb(string languageCode = "de-DE")
        {
            var receipts = await _receiptInformationManager.GetReceiptsInfoAsync();

            if (receipts.ErrorCode != ErrorCodes.None)
            {
                var httpStatus = receipts.ErrorCode.MapErrorCodeToHttpStatusCode();

                return StatusCode((int)httpStatus, receipts);
            }
            
            foreach (var receipt in receipts.Data!.Items)
            {
                var result = await _receiptInformationManager.GetReceiptAsync(receipt.Id, languageCode);
                if (result.ErrorCode != ErrorCodes.None)
                    continue;

                await _repository.AddReceiptAsync(result.Data!.ToReceipt());
            }
            
            return Ok(receipts);
        }
        
        [HttpGet("ReceiptInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceiptPageInfoDto>> Get()
        {
            var result = await _receiptInformationManager.GetReceiptsInfoAsync();

            if (result.ErrorCode != ErrorCodes.None)
            {
                var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();

                return StatusCode((int)httpStatus, result);
            }

            return Ok(result);
        }

        // GET: api/<ReceiptController>
        [HttpGet("{languageCode}/{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceiptPageInfoDto>> GetReceipt(string ticketId, string languageCode = "de-DE")
        {
            var result = await _receiptInformationManager.GetReceiptAsync(ticketId, languageCode);

            if (result.ErrorCode != ErrorCodes.None)
            {
                var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();

                return StatusCode((int)httpStatus, result);
            }

            return Ok(result);
        }

        [HttpGet("Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetToken()
        {
            var result = await _receiptInformationManager.GetAuthTokenAsync();

            if (result.ErrorCode != ErrorCodes.None)
            {
                var httpStatus = result.ErrorCode.MapErrorCodeToHttpStatusCode();

                return StatusCode((int)httpStatus, result);
            }

            return Ok(result);
        }

        // POST api/<ReceiptController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReceiptController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReceiptController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
