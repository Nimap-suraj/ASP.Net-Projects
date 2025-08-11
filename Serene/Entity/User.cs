using System.ComponentModel.DataAnnotations;

namespace Serene.Entity
{
    public class User
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        public string Roles { get; set; } = "Admin"; // User or Admin

        [Required, MinLength(10), MaxLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
