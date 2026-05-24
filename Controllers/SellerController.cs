using EComAPI.Data;
using EComAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EComAPI.Controllers
{

    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SellerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/sellers
        [HttpGet]
        [Route("api/sellers/GetSellers")]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            var sellers = await _context.Sellers
             .Select(s => new
             {
                 s.Id,
                 s.Name,
                 s.Email
             })
             .ToListAsync();

            return Ok(sellers);
        }

        // GET: api/sellers/{id}
        [HttpGet]
        [Route("api/sellers/GetSellerById/{id}")]
        public async Task<IActionResult> GetSeller(int id)
        {
            var seller = await _context.Sellers
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email
                })
                .FirstOrDefaultAsync();

            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        // POST: api/sellers
        [HttpPost]
[Route("api/sellers/CreateSeller")]
public async Task<IActionResult> CreateSeller(
    [FromBody] RegisterSellerDto dto)
{
    var existingSeller = await _context.Sellers
        .AnyAsync(x => x.Email == dto.Email);

    if (existingSeller)
    {
        return BadRequest(new
        {
            message = "Email already exists"
        });
    }

    var seller = new Seller
    {
        Name = dto.Name,

        Email = dto.Email,

        PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(dto.Password)
    };

    _context.Sellers.Add(seller);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        seller.Id,
        seller.Name,
        seller.Email
    });
}

        // PUT: api/sellers/{id}
        [HttpPut]
        [Route("api/sellers/UpdateSeller/{id}")]
        public async Task<IActionResult> UpdateSeller(
            int id,
            RegisterSellerDto updatedSeller)
        {
            if (id <= 0)
                return BadRequest("Invalid seller id");

            var seller = await _context.Sellers.FindAsync(id);

            if (seller == null)
                return NotFound();

            seller.Name = updatedSeller.Name;

            seller.Email = updatedSeller.Email;

            seller.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    updatedSeller.Password
                );

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Seller updated successfully"
            });
        }

        // DELETE: api/sellers/{id}
        [HttpDelete]
        [Route("api/sellers/DeleteSeller/{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
                return NotFound();

            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Seller deleted successfully" });
        }


        // LOGIN SELLER
        [HttpPost]
        [Route("api/sellers/LoginSeller")]
        public async Task<IActionResult> LoginSeller(
            [FromBody] LoginSellerDto dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x =>
                    x.Email == dto.Email);

            // CHECK EMAIL
            if (seller == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // VERIFY PASSWORD
            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    seller.PasswordHash
                );

            if (!isPasswordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // LOGIN SUCCESS
            return Ok(new
            {
                seller.Id,
                seller.Name,
                seller.Email
            });
        }
    }
}
