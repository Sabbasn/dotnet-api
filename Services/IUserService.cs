using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Dtos.Auth;
using dotnet_app.Models;

namespace dotnet_app.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> GetUser(int id);
        Task<bool> UserExists(string username);
    }
}