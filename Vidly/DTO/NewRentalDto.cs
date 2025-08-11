namespace Vidly.DTO
{
    public class NewRentalDto
    {
        public int CustomerId { get; set; }
        public List<int> MovieIds { get; set; }  // ✅ Not IEnumerable, but List is safer for model binding
    }
}
