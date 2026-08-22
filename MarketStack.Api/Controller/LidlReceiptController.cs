using MarketStack.Common.ApiBase;
using MarketStack.Common.ErrorHandling;
using MarketStack.Logic.Contracts.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketStack.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LidlReceiptController : ControllerBase
    {
        private readonly IReceiptLibraryService _receiptLibraryService;
        
        public LidlReceiptController(IReceiptLibraryService receiptLibraryService)
        {
            _receiptLibraryService = receiptLibraryService;
        }
        
        [HttpGet("Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetToken()
        {
            var result = await _receiptLibraryService.GetAuthTokenAsync();

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
