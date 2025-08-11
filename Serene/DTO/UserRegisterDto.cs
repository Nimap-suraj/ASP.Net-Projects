namespace Serene.DTO
{
    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //public string Roles { get; set; } 

    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}