namespace GenericRepositoryPattern.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public decimal  price { get; set; }

        // Foriegn key
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}