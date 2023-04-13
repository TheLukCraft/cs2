using Cosmonaut;
using Cosmonaut.Extensions;
using Domain.Entities;
using Domain.Entities.Cosmos;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CosmosPostRepository : ICosmosPostRepository
    {
        private readonly ICosmosStore<CosmosPost> cosmosStore;

        public CosmosPostRepository(ICosmosStore<CosmosPost> cosmosStore)
        {
            this.cosmosStore = cosmosStore;
        }

        public async Task<IEnumerable<CosmosPost>> GetAllAsync()
        {
            var posts = await cosmosStore.Query().ToListAsync();
            return posts;
        }

        public async Task<CosmosPost> GetByIdAsync(string id)
        {
            return await cosmosStore.FindAsync(id);
        }

        public async Task<CosmosPost> AddAsync(CosmosPost post)
        {
            post.Id = Guid.NewGuid().ToString();
            return await cosmosStore.AddAsync(post);
        }

        public async Task UpdatedAsync(CosmosPost post)
        {
            await cosmosStore.UpdateAsync(post);
        }

        public async Task DeleteAsync(CosmosPost post)
        {
            await cosmosStore.RemoveAsync(post);
        }
    }
}