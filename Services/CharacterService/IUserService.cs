using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.bin;

namespace dotnet_app.Services.CharacterService
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        List<User> AddUser(User user);
    }
}