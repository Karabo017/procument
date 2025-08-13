using Microsoft.AspNetCore.Mvc;
using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidsController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetBids()
        {
            var bids = await _bidService.GetAllBidsAsync();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidDto>> GetBid(Guid id)
        {
            var bid = await _bidService.GetBidByIdAsync(id);
            if (bid == null)
                return NotFound();

            return Ok(bid);
        }

        [HttpGet("by-supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetBidsBySupplier(Guid supplierId)
        {
            var bids = await _bidService.GetBidsBySupplierAsync(supplierId);
            return Ok(bids);
        }

        [HttpGet("by-tender/{tenderId}")]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetBidsByTender(Guid tenderId)
        {
            var bids = await _bidService.GetBidsByTenderAsync(tenderId);
            return Ok(bids);
        }

        [HttpPost("submit/{supplierId}")]
        public async Task<ActionResult<BidDto>> SubmitBid(Guid supplierId, BidSubmissionDto dto)
        {
            try
            {
                var bid = await _bidService.SubmitBidAsync(supplierId, dto);
                return CreatedAtAction(nameof(GetBid), new { id = bid.Id }, bid);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BidDto>> UpdateBid(Guid id, UpdateBidDto dto)
        {
            try
            {
                var bid = await _bidService.UpdateBidAsync(id, dto);
                if (bid == null)
                    return NotFound();

                return Ok(bid);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/withdraw/{supplierId}")]
        public async Task<IActionResult> WithdrawBid(Guid id, Guid supplierId)
        {
            var result = await _bidService.WithdrawBidAsync(id, supplierId);
            if (!result)
                return BadRequest("Cannot withdraw bid");

            return NoContent();
        }

        [HttpPost("{id}/review")]
        public async Task<IActionResult> ReviewBid(Guid id, [FromBody] ReviewBidRequest request)
        {
            var result = await _bidService.ReviewBidAsync(id, request.Status, request.Notes);
            if (!result)
                return NotFound();

            return Ok();
        }
    }

    public class ReviewBidRequest
    {
        public BidStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
