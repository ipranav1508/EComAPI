namespace EComAPI.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

    }

    public class RegisterSellerDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginSellerDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
