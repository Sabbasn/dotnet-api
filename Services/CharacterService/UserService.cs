using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.bin;

namespace dotnet_app.Services.CharacterService
{
    public class UserService : IUserService
    {
        private static List<User> users = new() {
            new User (),
            new User {Id=1, Name = "Merry"}
        };
        public List<User> AddUser(User user)
        {
            users.Add(user);
            return users;
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }
    }
}