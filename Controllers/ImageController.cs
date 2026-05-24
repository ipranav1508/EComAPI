using EComAPI.Data;  // Add this namespace for ApplicationDbContext
using EComAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ImageUploadApi.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;  // Add this line for the ApplicationDbContext

        // Inject ApplicationDbContext along with other services
        public ImageController(IWebHostEnvironment env, IConfiguration config, ApplicationDbContext context)
        {
            _env = env;
            _config = config;
            _context = context;  // Initialize _context
        }

        [HttpPost]
        [Route("api/images/UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var maxSizeMB = _config.GetValue<int>("FileUpload:MaxFileSizeMB");
            if (file.Length > maxSizeMB * 1024 * 1024)
                return BadRequest("File too large.");

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Save file information in the database
            var image = new Image
            {
                FileName = fileName
            };

            _context.Images.Add(image);  // Add image to the database context
            await _context.SaveChangesAsync();  // Save changes to the database

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fileUrl = $"{baseUrl}/images/{fileName}";

            return Ok(new { fileUrl });
        }
    }
}
