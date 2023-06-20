using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Dtos.Post;
using dotnet_app.Models;
using dotnet_app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dotnet_app.Controllers
{
    [Authorize]
    [ApiController]
    [Route("post")] 
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            if (post.Success == false)
            {
                return NotFound(post);
            }
            return Ok(post);
        }

        [HttpGet("getuserposts")]
        public async Task<IActionResult> GetUserPosts()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)!.Value); 
            var posts = await _postService.GetUserPosts();
            if (posts.Success == false)
            {
                return NotFound(posts);
            }
            return Ok(posts);
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            if (!posts.Success)
            {
                return NotFound(posts);
            }
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostDto newPost)
        {
            var post = await _postService.AddPost(newPost);
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var response = await _postService.DeletePost(id);
            if (response.Success == false)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostDto updatedPost)
        {
            var post = await _postService.UpdatePost(updatedPost);
            if (post.Success == false)
            {
                return BadRequest(post);
            }
            return Ok(post);
        }
    }


}