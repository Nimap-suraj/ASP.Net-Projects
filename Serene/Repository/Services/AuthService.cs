using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serene.Data;
using Serene.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using Serene.Repository.Interface;
using Serene.DTO;


namespace Serene.Repository.Services
{
    public class AuthService : IAuth
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,user.Roles),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User> Register(UserRegisterDto userDto)
        {
            // Check if email exists
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new ArgumentException("Email already registered");

            // Create and save user
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = HashPassword(userDto.Password),
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address,
                Roles = "User" // Default role
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public User Authenticate(string email,string password) {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if(user == null)
            {
                return null;
            }
            var hashInputPassword = HashPassword(password);
            if (user.Password != hashInputPassword)
                return null;

            return user;
        }
    }
}
