using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Receipt;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        [HttpGet("ReceiptInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ReceiptPageInfoDto>> Get()
        {
            LidlReceiptClient client = new LidlReceiptClient();
            var result = await client.GetReceiptsInfoAsync();

            if (result == null)
                return NoContent();
            
            return Ok(result);
        }

        // GET: api/<ReceiptController>
        [HttpGet("{languageCode}/{ticketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ReceiptPageInfoDto>> GetReceipt(string languageCode, string ticketId)
        {
            LidlReceiptClient client = new LidlReceiptClient();
            var result = await client.GetReceiptAsync(ticketId, languageCode);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<string>> GetToken()
        {
            LidlReceiptClient client = new LidlReceiptClient();
            var result = await client.GetAuthTokenAsync();
            
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
