namespace Vidly.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // User and Admin
        public string Role { get; set; }
    }
}
