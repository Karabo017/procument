using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VWProcurement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload/profile")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            // Validate file type
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Only image files are allowed");

            // Validate file size (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File size cannot exceed 5MB");

            try
            {
                // Create uploads directory
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/profiles/{fileName}";
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpPost("upload/tender")]
        [Authorize]
        public async Task<IActionResult> UploadTenderDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            // Validate file size (max 50MB)
            if (file.Length > 50 * 1024 * 1024)
                return BadRequest("File size cannot exceed 50MB");

            try
            {
                // Create uploads directory
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "tenders");
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/tenders/{fileName}";
                return Ok(new 
                { 
                    url = fileUrl,
                    fileName = file.FileName,
                    size = file.Length,
                    contentType = file.ContentType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpPost("upload/bid")]
        [Authorize]
        public async Task<IActionResult> UploadBidDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            // Validate file size (max 50MB)
            if (file.Length > 50 * 1024 * 1024)
                return BadRequest("File size cannot exceed 50MB");

            try
            {
                // Create uploads directory
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "bids");
                Directory.CreateDirectory(uploadsPath);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/bids/{fileName}";
                return Ok(new 
                { 
                    url = fileUrl,
                    fileName = file.FileName,
                    size = file.Length,
                    contentType = file.ContentType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpGet("download/{type}/{fileName}")]
        public IActionResult DownloadFile(string type, string fileName)
        {
            var allowedTypes = new[] { "profiles", "tenders", "bids" };
            if (!allowedTypes.Contains(type.ToLower()))
                return BadRequest("Invalid file type");

            var filePath = Path.Combine(_environment.WebRootPath, "uploads", type, fileName);
            
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = GetContentType(fileName);
            
            return File(fileBytes, contentType, fileName);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }
    }
}
