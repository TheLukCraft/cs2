using Application.Services;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class CSGOContext : IdentityDbContext<ApplicationUser>
    {
        private readonly UserResloverService userService;

        public CSGOContext(DbContextOptions<CSGOContext> options, UserResloverService userService) : base(options)
        {
            this.userService = userService;
        }

        public DbSet<Post>? Posts { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;
                ((AuditableEntity)entityEntry.Entity).LastModifiedBy = userService.GetUser();

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = userService.GetUser();
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}