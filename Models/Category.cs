namespace EComAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class CategoryDto
    {
        public string Name { get; set; }
            = string.Empty;
    }
}
