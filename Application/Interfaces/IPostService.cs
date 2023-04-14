using Application.Dto;

namespace Application.Interfaces
{
    public interface IPostService
    {
        IQueryable<PostDto> GetAllPosts();

        Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize);

        Task<int> GetAllPostsCountAsync();

        Task<PostDto> GetPostByIdAsync(int id);

        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);

        Task UpdatePostAsync(UpdatePostDto updatePost);

        Task DeletePostAsync(int id);
    }
}