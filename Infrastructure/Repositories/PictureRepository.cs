using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly CSContext context;

        public PictureRepository(CSContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Picture>> GetByPostIdAsync(int postId)
        {
            return await context.Pictures
                .Include(x => x.Posts)
                .Where(x => x.Posts.Select(x => x.Id)
                .Contains(postId)).ToListAsync();
        }

        public async Task<Picture> GetByIdAsync(int id)
        {
            return await context.Pictures.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task SetMainPictureAsync(int postId, int id)
        {
            var currentMainPicture = await context.Pictures.Include(x => x.Posts)
                .Where(x => x.Posts.Select(x => x.Id).Contains(postId))
                .SingleOrDefaultAsync(x => x.Main);

            currentMainPicture.Main = false;

            var newMainPicture = await context.Pictures.SingleOrDefaultAsync(x => x.Id == id);
            newMainPicture.Main = true;

            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<Picture> AddAsync(Picture picture)
        {
            var createdPicture = await context.Pictures.AddAsync(picture);
            await context.SaveChangesAsync();

            return createdPicture.Entity;
        }

        public async Task UpdatedAsync(Picture picture)
        {
            context.Pictures.Update(picture);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Picture picture)
        {
            context.Pictures.Remove(picture);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}