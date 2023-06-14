using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Models;
using dotnet_app.bin;

namespace dotnet_app.Services.CharacterService
{
    public class UserService : IUserService
    {
        private static List<User> users = new() {
            new User (),
            new User {Id=1, Name = "Merry"}
        };
        public async Task<ServiceResponse<List<User>>> AddUser(User user)
        {
            var serviceResponse = new ServiceResponse<List<User>>();
            users.Add(user);
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<User>>> GetAllUsers()
        {
            var serviceResponse = new ServiceResponse<List<User>>();
            serviceResponse.Data = users;
            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> GetUserById(int id)
        {
            var serviceResponse = new ServiceResponse<User>();
            var user = users.FirstOrDefault(u => u.Id == id);
            serviceResponse.Data = user;
            return serviceResponse;
        }
    }
}