using Microsoft.AspNetCore.Mvc;
using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyersController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuyerDto>>> GetBuyers()
        {
            var buyers = await _buyerService.GetAllBuyersAsync();
            return Ok(buyers);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BuyerDto>>> GetActiveBuyers()
        {
            var buyers = await _buyerService.GetActiveBuyersAsync();
            return Ok(buyers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuyerDto>> GetBuyer(Guid id)
        {
            var buyer = await _buyerService.GetBuyerByIdAsync(id);
            if (buyer == null)
                return NotFound();

            return Ok(buyer);
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<BuyerDto>> GetBuyerByEmail(string email)
        {
            var buyer = await _buyerService.GetBuyerByEmailAsync(email);
            if (buyer == null)
                return NotFound();

            return Ok(buyer);
        }

        [HttpPost]
        public async Task<ActionResult<BuyerDto>> CreateBuyer(CreateBuyerDto dto)
        {
            try
            {
                var buyer = await _buyerService.CreateBuyerAsync(dto);
                return CreatedAtAction(nameof(GetBuyer), new { id = buyer.Id }, buyer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BuyerDto>> UpdateBuyer(Guid id, UpdateBuyerDto dto)
        {
            try
            {
                var buyer = await _buyerService.UpdateBuyerAsync(id, dto);
                if (buyer == null)
                    return NotFound();

                return Ok(buyer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyer(Guid id)
        {
            var result = await _buyerService.DeleteBuyerAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
