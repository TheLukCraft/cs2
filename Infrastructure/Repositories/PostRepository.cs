using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CSGOContext context;

        public PostRepository(CSGOContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await context.Posts.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await context.Posts.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Post> AddAsync(Post post)
        {
            var createdPost = await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return createdPost.Entity;
        }

        public async Task UpdatedAsync(Post post)
        {
            context.Posts.Update(post);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Post post)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}