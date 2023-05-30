using Application.Dto.Map;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly IMapService mapService;

        public MapsController(IMapService mapService)
        {
            this.mapService = mapService;
        }

        [SwaggerOperation(Summary = "Retrieves all maps")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<MapDto>>> GetAllAsync()
        {
            var maps = await mapService.GetAllMapsAsync();
            return Ok(maps);
        }

        [SwaggerOperation(Summary = "Retrieves a specific map")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var map = await mapService.GetMapByIdAsync(id);
            if (map == null)
                return NotFound();

            return Ok(new Response<MapDto>(map));
        }

        [SwaggerOperation(Summary = "Add new map")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Add(CreateMapDto newMap)
        {
            var map = await mapService.AddNewMapAsync(newMap);
            return Created($"maps/{map.Id}", new Response<MapDto>(map));
        }

        [SwaggerOperation(Summary = "Update a existing map")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(UpdateMapDto updateMap)
        {
            await mapService.UpdateMapAsync(updateMap);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific map")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await mapService.DeleteMapAsync(id);
            return NoContent();
        }
    }
}