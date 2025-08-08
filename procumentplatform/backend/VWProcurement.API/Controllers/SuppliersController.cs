using Microsoft.AspNetCore.Mvc;
using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActiveSuppliers()
        {
            var suppliers = await _supplierService.GetActiveSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<SupplierDto>> GetSupplierByEmail(string email)
        {
            var supplier = await _supplierService.GetSupplierByEmailAsync(email);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier(CreateSupplierDto dto)
        {
            try
            {
                var supplier = await _supplierService.CreateSupplierAsync(dto);
                return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierDto>> UpdateSupplier(int id, UpdateSupplierDto dto)
        {
            try
            {
                var supplier = await _supplierService.UpdateSupplierAsync(id, dto);
                if (supplier == null)
                    return NotFound();

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
