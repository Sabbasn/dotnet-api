using dotnet_app.Dtos.Post;
using dotnet_app.Models;

namespace dotnet_app.Services
{
    public interface IPostService
    {
        Task<ServiceResponse<List<GetPostDto>>> GetUserPosts();
        Task<ServiceResponse<List<GetPostDto>>> GetAllPosts();
        Task<ServiceResponse<GetPostDto>> GetPost(int id);
        Task<ServiceResponse<AddPostDto>> AddPost(AddPostDto newPost);
        Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto newPost);
        Task<ServiceResponse<GetPostDto>> DeletePost(int id);
    }
}