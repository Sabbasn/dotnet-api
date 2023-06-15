using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Models;

namespace dotnet_app.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name";
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set;} = Array.Empty<byte>();
        public string Email { get; set; } = "Email";
        public List<Post>? Posts { get; set; }
    }
}