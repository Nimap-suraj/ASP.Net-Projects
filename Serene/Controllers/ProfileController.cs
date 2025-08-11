using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serene.Entity;
using Serene.Repository.Interface;
using System.Security.Claims;

namespace Serene.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetMyProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _profileService.GetProfile(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // ✅ Admin can get any user profile
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserProfile(int id)
        {
            var user = _profileService.GetProfileByAdmin(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // ✅ Update current user's profile
        [HttpPut]
        [Authorize]
        public IActionResult UpdateMyProfile([FromBody] User updatedUser)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _profileService.UpdateMyProfile(userId, updatedUser);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
