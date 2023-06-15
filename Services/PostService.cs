using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_app.Data;
using dotnet_app.Dtos.Post;
using dotnet_app.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_app.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public PostService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<AddPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new ServiceResponse<AddPostDto>();
            try
            {
                Post post = _mapper.Map<Post>(newPost);
                post.Title = newPost.Title;
                post.Text = newPost.Text;
                post.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == newPost.UserId)
                ?? throw new Exception(string.Format("UserID '{0}' does not exist!", newPost.UserId));
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                serviceResponse.Data = newPost;
                serviceResponse.Message = "Successfully added Post!";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> DeletePost(int id)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            {
                Post post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception(string.Format("PostID '{0}' does not exist!", id));
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
                serviceResponse.Message = "Successfully deleted post!";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            var serviceResponse = new ServiceResponse<List<GetPostDto>>();
            try
            {
                List<Post> posts = await _context.Posts.ToListAsync();
                serviceResponse.Data = posts.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message= ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> GetPost(int id)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            {
                Post post = 
                    await _context.Posts.FirstOrDefaultAsync(p => p.Id == id) 
                    ?? throw new Exception(string.Format("PostID '{0}' does not exist!", id));
                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto newPost)
        {
            var serviceResponse = new ServiceResponse<GetPostDto>();
            try
            {
                Post post = await _context.Posts.FirstOrDefaultAsync(p => newPost.Id == p.Id)
                    ?? throw new Exception(string.Format("PostID '{0}' does not exist!", newPost.Id));
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPostDto>(post);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}