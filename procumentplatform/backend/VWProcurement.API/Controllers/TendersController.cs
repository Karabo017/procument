using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VWProcurement.Data;
using VWProcurement.Core.Models;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TendersController : ControllerBase
    {
        private readonly VWProcurementDbContext _context;

        public TendersController(VWProcurementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTenders()
        {
            try
            {
                var tenders = await _context.Tenders
                    .Include(t => t.Buyer) // Buyer is a User entity
                    .Include(t => t.Category)
                    .Where(t => t.Status == TenderStatus.Open) // Only show active tenders
                    .Select(t => new
                    {
                        id = t.Id,
                        tenderNumber = t.TenderNumber,
                        title = t.Title,
                        description = t.Description,
                        requirements = t.Requirements,
                        estimatedValue = t.EstimatedValue,
                        status = t.Status,
                        publishDate = t.PublishDate,
                        closingDate = t.ClosingDate,
                        awardDate = t.AwardDate,
                        createdAt = t.CreatedAt,
                        updatedAt = t.UpdatedAt,
                        category = new
                        {
                            id = t.Category.Id,
                            name = t.Category.Name,
                            isActive = t.Category.IsActive
                        },
                        buyer = _context.Buyers
                            .Where(b => b.UserId == t.BuyerId)
                            .Select(b => new
                            {
                                id = b.Id,
                                organizationName = b.OrganizationName,
                                isVerified = b.IsVerified,
                                user = new
                                {
                                    email = t.Buyer.Email,
                                    userType = t.Buyer.Role.ToString()
                                }
                            })
                            .FirstOrDefault(),
                        // Calculate days remaining
                        daysRemaining = (t.ClosingDate - DateTime.UtcNow).TotalDays,
                        // Calculate if closing soon (within 7 days)
                        isClosingSoon = (t.ClosingDate - DateTime.UtcNow).TotalDays <= 7
                    })
                    .OrderByDescending(t => t.createdAt)
                    .ToListAsync();

                // Update status based on closing date and format for frontend
                var tendersWithUpdatedStatus = tenders.Select(t => new
                {
                    t.id,
                    t.tenderNumber,
                    t.title,
                    t.description,
                    t.requirements,
                    estimatedValue = t.estimatedValue,
                    status = t.daysRemaining < 0 ? "Closed" : (t.isClosingSoon ? "Closing Soon" : t.status.ToString()),
                    publishDate = t.publishDate?.ToString("yyyy-MM-dd"),
                    closingDate = t.closingDate.ToString("yyyy-MM-dd"),
                    t.awardDate,
                    t.createdAt,
                    t.updatedAt,
                    category = t.category.name,
                    location = t.buyer?.organizationName,
                    buyerName = t.buyer?.organizationName,
                    isVerified = t.buyer?.isVerified ?? false,
                    daysRemaining = Math.Max(0, (int)t.daysRemaining),
                    t.isClosingSoon,
                    // Add tender type based on estimated value
                    tenderType = t.estimatedValue switch
                    {
                        null => "Standard",
                        var val when val < 100000 => "Small",
                        var val when val < 1000000 => "Medium",
                        _ => "Large"
                    }
                }).ToList();

                return Ok(new
                {
                    success = true,
                    count = tendersWithUpdatedStatus.Count,
                    data = tendersWithUpdatedStatus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching tenders",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTender(Guid id)
        {
            try
            {
                var tender = await _context.Tenders
                    .Include(t => t.Buyer)
                    .Include(t => t.Category)
                    .Include(t => t.Bids)
                        .ThenInclude(b => b.Supplier)
                            .ThenInclude(s => s.User)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tender == null)
                {
                    return NotFound(new { message = "Tender not found" });
                }

                var result = new
                {
                    id = tender.Id,
                    tenderNumber = tender.TenderNumber,
                    title = tender.Title,
                    description = tender.Description,
                    requirements = tender.Requirements,
                    estimatedValue = tender.EstimatedValue,
                    status = tender.Status,
                    publishDate = tender.PublishDate,
                    closingDate = tender.ClosingDate,
                    awardDate = tender.AwardDate,
                    createdAt = tender.CreatedAt,
                    updatedAt = tender.UpdatedAt,
                    category = new
                    {
                        id = tender.Category.Id,
                        name = tender.Category.Name,
                        isActive = tender.Category.IsActive
                    },
                    buyer = _context.Buyers
                        .Where(b => b.UserId == tender.BuyerId)
                        .Select(b => new
                        {
                            id = b.Id,
                            organizationName = b.OrganizationName,
                            isVerified = b.IsVerified,
                            user = new
                            {
                                email = tender.Buyer.Email,
                                userType = tender.Buyer.Role.ToString()
                            }
                        })
                        .FirstOrDefault(),
                    bids = tender.Bids.Select(b => new
                    {
                        id = b.Id,
                        bidNumber = b.BidNumber,
                        bidAmount = b.BidAmount,
                        status = b.Status,
                        submissionDate = b.SubmissionDate,
                        validityPeriod = b.ValidityPeriod,
                        supplier = new
                        {
                            id = b.Supplier.Id,
                            companyName = b.Supplier.CompanyName,
                            isVerified = b.Supplier.IsVerified
                        }
                    }).ToList(),
                    // Calculate days remaining
                    daysRemaining = Math.Max(0, (tender.ClosingDate - DateTime.UtcNow).TotalDays),
                    isClosingSoon = (tender.ClosingDate - DateTime.UtcNow).TotalDays <= 7
                };

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching tender details",
                    error = ex.Message
                });
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.TenderCategories
                    .Where(c => c.IsActive)
                    .Select(c => new
                    {
                        id = c.Id,
                        name = c.Name,
                        isActive = c.IsActive,
                        createdAt = c.CreatedAt
                    })
                    .OrderBy(c => c.name)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = categories.Count,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching categories",
                    error = ex.Message
                });
            }
        }
    }
}
