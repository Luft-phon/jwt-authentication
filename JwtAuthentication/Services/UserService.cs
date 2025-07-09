using JwtAuthentication.Data;
using JwtAuthentication.Entity;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtAuthentication.Services
{
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context; 
        public UserService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<User?> RegisterAsync(UserDTO dto)
        {
            var users = await _context.Users.AnyAsync(u => u.Username == dto.UserName); // Check if the user already exists
            if (users == true)
            {
                return null;
            }

            var user = new User();
        var hashPassword = new PasswordHasher<User>()
        .HashPassword(user, dto.Password);
            user.Username = dto.UserName;
            user.PasswordHash = hashPassword;
             await _context.Users.AddAsync(user); 
            await _context.SaveChangesAsync();  
            return user;
        }
        public async Task<TokenResponseDTO> LoginAsync(UserDTO dto)
        {
            var users = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.UserName); // Check if the user exists
            if (users == null)
            {
                return null;
            }

            // Verify the hashed password
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(users, users.PasswordHash, dto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var response = new TokenResponseDTO
            {
                AccessToken = CreateToken(users),
                RefreshToken = await GenerateAndSaveToken(users)
            };

            return response;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role) // Thêm claim cho vai trò của người dùng
            };
            var secret_key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(secret_key, SecurityAlgorithms.HmacSha512);      // dùng thuật toán HmacSha512 để ký token
            var tokenDescriptor = new JwtSecurityToken(                     // đóng gói thông tin vào token
                    issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: _configuration.GetValue<string>("AppSettings:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);  //chuyển token thành string
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber); // Chuyển đổi mảng byte thành chuỗi Base64
        }
        private async Task<string> GenerateAndSaveToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken 
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }
        public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO dto)
        {
            var user = await ValidateRefreshToken(dto.UserID, dto.RefreshToken);
            if (user is null)
            {
                return null;
            }
            var response = new TokenResponseDTO
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveToken(user)
            };
            return response;
        }
    }
}
