using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MapRepository : IMapRepository
    {
        private readonly CSContext context;

        public MapRepository(CSContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Map>> GetAllAsync()
        {
            return await context.Maps.ToListAsync();
        }

        public Task<Map> GetByIdAsync(int id)
        {
            return context.Maps.SingleOrDefaultAsync(map => map.Id == id);
        }

        public async Task<Map> AddAsync(Map map)
        {
            var addedMap = await context.Maps.AddAsync(map);
            await context.SaveChangesAsync();
            return addedMap.Entity;
        }

        public async Task UpdatedAsync(Map map)
        {
            context.Maps.Update(map);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Map map)
        {
            context.Maps.Remove(map);
            await context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}