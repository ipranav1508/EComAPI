using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EComAPI.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; } // not mapped to DB
    }
}

