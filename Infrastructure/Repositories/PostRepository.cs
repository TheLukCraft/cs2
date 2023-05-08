using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CSContext context;

        public PostRepository(CSContext context)
        {
            this.context = context;
        }

        public IQueryable<Post> GetAll()
        {
            return context.Posts.AsQueryable();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy)
        {
            return await context.Posts
                .Where(m => m.Title.ToLower().Contains(filterBy.ToLower()) || m.Content.ToLower().Contains(filterBy.ToLower()))
                .OrderByPropertyName(sortField, ascending)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public async Task<int> GetAllCountAsync(string filterBy)
        {
            return await context.Posts
                .Where(m => m.Title.ToLower()
                .Contains(filterBy.ToLower()) || m.Content.ToLower()
                .Contains(filterBy.ToLower()))
                .CountAsync();
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