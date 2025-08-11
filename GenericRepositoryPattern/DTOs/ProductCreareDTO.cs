namespace GenericRepositoryPattern.DTOs
{
    public class ProductCreareDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal price { get; set; }
        public int CategoryId { get; set; }
    }
}
