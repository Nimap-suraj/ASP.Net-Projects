using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vidly.Data;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("","user is null");
                return View(model);
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,ClaimTypes.Email),
                new Claim(ClaimTypes.Role,ClaimTypes.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ Store in secure HttpOnly cookie
            Response.Cookies.Append("jwtToken", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in production (HTTPS)
                Expires = DateTime.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("userEmail", user.Email);
            Response.Cookies.Append("userRole", user.Role);

            return RedirectToAction("Index", "Customer");
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            Response.Cookies.Delete("userEmail");
            Response.Cookies.Delete("userRole");

            return RedirectToAction("Login");
        }

    }
}
