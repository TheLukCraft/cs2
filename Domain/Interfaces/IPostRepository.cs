using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        IQueryable<Post> GetAll();

        Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize);

        Task<int> GetAllCountAsync();

        Task<Post> GetByIdAsync(int id);

        Task<Post> AddAsync(Post post);

        Task UpdatedAsync(Post post);

        Task DeleteAsync(Post post);
    }
}