using Application.Dto.Map;

namespace Application.Interfaces
{
    public interface IMapService
    {
        Task<IEnumerable<MapDto>> GetAllMapsAsync();

        Task<MapDto> GetMapByIdAsync(int Id);

        Task<MapDto> AddNewMapAsync(CreateMapDto map);

        Task UpdateMapAsync(UpdateMapDto map);

        Task DeleteMapAsync(int Id);
    }
}