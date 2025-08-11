using System.Text.Json.Serialization;

namespace GenericRepositoryPattern.Entity
{
    public class Category
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty ;
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
