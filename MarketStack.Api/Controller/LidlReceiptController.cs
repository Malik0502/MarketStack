using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Logic.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LidlReceiptController : ControllerBase
    {
        private readonly IReceiptInformationManager _receiptInformationManager;

        public LidlReceiptController(IReceiptInformationManager receiptInformationManager)
        {
            _receiptInformationManager = receiptInformationManager;
        }

        [HttpGet("ReceiptInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ReceiptPageInfoDto>> Get()
        {
            var result = await _receiptInformationManager.GetReceiptsInfoAsync();

            if (result == null)
                return NoContent();
            
            return Ok(result);
        }

        // GET: api/<ReceiptController>
        [HttpGet("{languageCode}/{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ReceiptPageInfoDto>> GetReceipt(string ticketId, string languageCode = "de-DE")
        {
            var result = await _receiptInformationManager.GetReceiptAsync(ticketId, languageCode);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<string>> GetToken()
        {
            var result = await _receiptInformationManager.GetAuthTokenAsync();
            
            if (result == null)
                return NoContent();
            
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
