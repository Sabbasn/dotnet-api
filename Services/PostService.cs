using System.Security.Claims;
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
        private readonly IHttpContextAccessor _accessor;

        public PostService(IMapper mapper, DataContext context, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _context = context;
            _accessor = accessor;
        }

        private int GetUserId() => int.Parse(_accessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<AddPostDto>> AddPost(AddPostDto newPost)
        {
            var serviceResponse = new ServiceResponse<AddPostDto>();
            try
            {
                Post post = _mapper.Map<Post>(newPost);
                post.Title = newPost.Title;
                post.Text = newPost.Text;
                post.TimePosted = DateTime.Now;
                post.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId())
                    ?? throw new Exception(string.Format("UserID '{0}' does not exist!", GetUserId()));
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                serviceResponse.Data = newPost;
                serviceResponse.Message = "Successfully added post!";
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
                Post post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id && p.User!.Id == GetUserId())
                    ?? throw new Exception(string.Format("PostID '{0}' does not exist or the user is unauthorized.", id));
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
                List<Post> posts = await _context.Posts.Where(p => p.User!.Id == GetUserId()).ToListAsync();
                serviceResponse.Data = posts.Select(_mapper.Map<GetPostDto>).ToList();
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
                    await _context.Posts.FirstOrDefaultAsync(p => p.Id == id && p.User!.Id == GetUserId())
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
                Post post = await _context.Posts
                    .Where(p => GetUserId() == p.User!.Id)
                    .FirstOrDefaultAsync(p => p.Id == newPost.Id)
                    ?? throw new Exception(string.Format("The user is not authorized to modify this post"));
                
                post.Text = string.IsNullOrEmpty(newPost.Text) ? post.Text : newPost.Text;
                post.Title= string.IsNullOrEmpty(newPost.Title) ? post.Title : newPost.Title;

                _context.Update(post);
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