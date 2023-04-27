using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly CSGOContext context;

        public AttachmentRepository(CSGOContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Attachment>> GetByPostIdAsync(int postId)
        {
            return await context.Attachments
                .Include(x => x.Posts)
                .Where(x => x.Posts.Select(x => x.Id)
                .Contains(postId)).ToListAsync();
        }

        public async Task<Attachment> GetByIdAsync(int id)
        {
            return await context.Attachments.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Attachment> AddAsync(Attachment attachment)
        {
            var createdAttachment = await context.Attachments.AddAsync(attachment);
            await context.SaveChangesAsync();
            return createdAttachment.Entity;
        }

        public async Task DeleteAsync(Attachment picture)
        {
            context.Attachments.Remove(picture);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}