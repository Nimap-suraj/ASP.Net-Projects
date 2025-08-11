using Serene.DTO;
using Serene.Entity;

namespace Serene.Repository.Interface
{
    public interface IAuth
    {
        public Task<User> Register(UserRegisterDto userDto);
        public User Authenticate(string email, string password);
        public string GenerateToken(User user);
    }
}
