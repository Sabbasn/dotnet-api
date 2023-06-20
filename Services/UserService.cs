using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using dotnet_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using dotnet_app.Dtos.Auth;

namespace dotnet_app.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == username.ToLower());
            
            if(user is null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Login failed.";
            }
            else 
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if(await UserExists(user.Name))
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }
            CreatePasswordHash(password, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            response.Data = user.Id;
            return response;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUser(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception($"UserID: {id} does not exist!");
            GetUserDto userDto = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };
            response.Data = userDto;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Name.ToLower() == username.ToLower())) 
            {
                return true;
            }
            return false;
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt) 
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);
        } 

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value 
            ?? throw new Exception("AppSettings.Token is null!");

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}