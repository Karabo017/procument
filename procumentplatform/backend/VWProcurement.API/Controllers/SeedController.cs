using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Models;
using VWProcurement.Data;
using VWProcurement.API;
using VWProcurement.API.Services;

namespace VWProcurement.API.Controllers
{
    // Uses Services.SampleDataSeeder for idempotent seed

    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly VWProcurementDbContext _context;

        public SeedController(VWProcurementDbContext context)
        {
            _context = context;
        }

        [HttpPost("sample-data")]
        public async Task<ActionResult> SeedSampleData()
        {
            try
            {
                var seeder = new SampleDataSeeder();
                var summary = await seeder.SeedAsync(_context);
                return Ok(new { success = true, message = "Sample data created or verified successfully", created = new { categories = summary.Categories, users = summary.Users, buyers = summary.Buyers, managers = summary.Managers, tenders = summary.TendersCreated } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating sample data", error = ex.Message });
            }
        }
    }
}
