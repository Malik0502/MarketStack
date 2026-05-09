using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Receipt;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        // GET: api/<ReceiptController>
        [HttpGet("ReceiptInfo")]
        public async Task<ActionResult<ReceiptPageInfoDto>> Get()
        {
            LidlReceiptClient client = new LidlReceiptClient();
            var result = await client.GetReceiptsAsync();

            return Ok(result);
        }

        // GET api/<ReceiptController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
