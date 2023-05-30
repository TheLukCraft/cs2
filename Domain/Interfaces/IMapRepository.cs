using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMapRepository
    {
        Task<IEnumerable<Map>> GetAllAsync();

        Task<Map> GetByIdAsync(int id);

        Task<Map> AddAsync(Map map);

        Task UpdatedAsync(Map map);

        Task DeleteAsync(Map map);
    }
}