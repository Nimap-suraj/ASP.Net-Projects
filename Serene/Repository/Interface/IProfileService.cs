using Serene.Entity;

namespace Serene.Repository.Interface
{
    public interface IProfileService
    {
        User? GetProfile(int userId);
        User? GetProfileByAdmin(int Id);
        User? UpdateMyProfile(int userId, User updatedUser);
    }
}
