using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetAll();

        Post GetById(int id);

        Post Add(Post post);

        void Updated(Post post);

        void Delete(Post post);
    }
}