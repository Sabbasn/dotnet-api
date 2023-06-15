using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.Dtos.Post
{
    public class AddPostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime TimePosted { get; set; } = DateTime.Now;
        public int UserId { get; set; }
    }
}