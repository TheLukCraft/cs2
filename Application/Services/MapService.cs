using Application.Dto.Map;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class MapService : IMapService
    {
        private readonly IMapRepository mapRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public MapService(IMapRepository mapRepository, IMapper mapper, ILogger logger)
        {
            this.mapRepository = mapRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<MapDto>> GetAllMapsAsync()
        {
            var maps = await mapRepository.GetAllAsync();
            return mapper.Map<IEnumerable<MapDto>>(maps);
        }

        public async Task<MapDto> GetMapByIdAsync(int Id)
        {
            var map = await mapRepository.GetByIdAsync(Id);
            return mapper.Map<MapDto>(map);
        }

        public async Task<MapDto> AddNewMapAsync(CreateMapDto map)
        {
            var newMap = mapper.Map<Map>(map);
            var result = await mapRepository.AddAsync(newMap);
            return mapper.Map<MapDto>(result);
        }

        public async Task UpdateMapAsync(UpdateMapDto map)
        {
            var existingMap = await mapRepository.GetByIdAsync(map.Id);
            var updatedMap = mapper.Map(map, existingMap);
            mapRepository.UpdatedAsync(updatedMap);
        }

        public async Task DeleteMapAsync(int Id)
        {
            var map = await mapRepository.GetByIdAsync(Id);
            await mapRepository.DeleteAsync(map);
        }
    }
}