using GenericRepositoryPattern.Entity;

namespace GenericRepositoryPattern.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
