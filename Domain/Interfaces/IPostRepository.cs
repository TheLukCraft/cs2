using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();

        Task<Post> GetByIdAsync(int id);

        Task<Post> AddAsync(Post post);

        Task UpdatedAsync(Post post);

        Task DeleteAsync(Post post);
    }
}