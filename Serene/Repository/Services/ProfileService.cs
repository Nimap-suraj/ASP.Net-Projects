using Serene.Data;
using Serene.Entity;
using Serene.Repository.Interface;

namespace Serene.Repository.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }
        public User? GetProfile(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public User? GetProfileByAdmin(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public User? UpdateMyProfile(int userId, User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if(user != null)
            {
                user.Name = updatedUser.Name;
                user.Address = updatedUser.Address;
                user.PhoneNumber = updatedUser.PhoneNumber;
            }
            _context.SaveChanges();
            return user;
        }
    }
}
