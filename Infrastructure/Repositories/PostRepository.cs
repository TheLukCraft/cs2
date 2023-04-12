using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private static readonly ISet<Post> posts = new HashSet<Post>()
        {
            new Post(1, "test 1", "treść 1"),
            new Post(2, "test 2", "treść 2"),
            new Post(3, "test 3", "treść 3"),
        };

        public IEnumerable<Post> GetAll()
        {
            return posts;
        }

        public Post GetById(int id)
        {
            return posts.SingleOrDefault(x => x.Id == id);
        }

        public Post Add(Post post)
        {
            post.Id = posts.Count() + 1;
            post.Created = DateTime.UtcNow;
            posts.Add(post);
            return post;
        }

        public void Updated(Post post)
        {
            post.LastModified = DateTime.UtcNow;
        }

        public void Delete(Post post)
        {
            posts.Remove(post);
        }
    }
}