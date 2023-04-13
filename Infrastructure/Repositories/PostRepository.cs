using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CSGOContext context;

        public PostRepository(CSGOContext context)
        {
            this.context = context;
        }

        public IEnumerable<Post> GetAll()
        {
            return context.Posts;
        }

        public Post GetById(int id)
        {
            return context.Posts.SingleOrDefault(x => x.Id == id);
        }

        public Post Add(Post post)
        {
            context.Posts.Add(post);
            context.SaveChanges();
            return post;
        }

        public void Updated(Post post)
        {
            context.Posts.Update(post);
            context.SaveChanges();
        }

        public void Delete(Post post)
        {
            context.Posts.Remove(post);
            context.SaveChanges();
        }
    }
}