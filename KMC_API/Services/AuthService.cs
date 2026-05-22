using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KMC_API.Data;
using KMC_API.DTOs;
using KMC_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KMC_API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> Register(RegisterRequest request);
        Task<AuthResponse?> Login(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly string _jwtKey;

        public AuthService(AppDbContext db, string jwtKey)
        {
            _db = db;
            _jwtKey = jwtKey;
        }

        public async Task<AuthResponse?> Register(RegisterRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email))
                return null;

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role == "Organizer" ? "Organizer" : "Public"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return BuildAuthResponse(user);
        }

        public async Task<AuthResponse?> Login(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return BuildAuthResponse(user);
        }

        private AuthResponse BuildAuthResponse(User user) => new AuthResponse
        {
            Token = GenerateToken(user),
            Name = user.Name,
            Role = user.Role,
            UserId = user.Id
        };

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}