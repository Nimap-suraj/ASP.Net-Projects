using System.Text.Json.Serialization;

namespace GenericRepositoryPattern.Entity
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public decimal  price { get; set; }

        // Foriegn key
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}