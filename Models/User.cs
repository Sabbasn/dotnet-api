using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.bin
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name";
        public string Password { get; set; } = "Password";
        public string Email { get; set; } = "Email";
    }
}