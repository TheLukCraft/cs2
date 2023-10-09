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

        /// <summary>
        /// Retrieves all maps asynchronously.
        /// </summary>
        /// <Response code="200">(OK) with the collection of maps if retrieval is successful.</Response>
        [SwaggerOperation(Summary = "Retrieves all maps")]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<MapDto>>> GetAllAsync()
        {
            var maps = await mapService.GetAllMapsAsync();
            return Ok(maps);
        }

        /// <summary>
        /// Retrieves a specific map by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the map to retrieve.</param>
        /// <Reseponse code="404">(Not Found) if the map with the specified ID does not exist.</Reseponse>
        /// <Response code="200">(OK) with the MapDto object if the retrieval is successful.</Response>
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

        /// <summary>
        /// Adds a new map asynchronously.
        /// </summary>
        /// <param name="newMap">The CreateMapDto object containing the details of the new map.</param>
        /// <Response code="401">(Unauthorized) if the user is not authorized (requires Admin role).</Response>
        /// <Resposne code="201">(Created) with the MapDto object if the map is successfully added.</Resposne>
        [SwaggerOperation(Summary = "Add new map")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Add(CreateMapDto newMap)
        {
            var map = await mapService.AddNewMapAsync(newMap);
            return Created($"maps/{map.Id}", new Response<MapDto>(map));
        }

        /// <summary>
        /// Updates an existing map asynchronously.
        /// </summary>
        /// <param name="updateMap">The UpdateMapDto object containing the updated details of the map.</param>
        /// <Response code="401">(Unauthorized) if the user is not authorized (requires Admin role).</Response>
        /// <Response code="204"> (No Content) if the map is successfully updated.</Response>
        [SwaggerOperation(Summary = "Update a existing map")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(UpdateMapDto updateMap)
        {
            await mapService.UpdateMapAsync(updateMap);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific map by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the map to delete.</param>
        /// <Response code="401">(Unauthorized) if the user is not authorized (requires Admin role).</Response>
        /// <Response code="204">(No Content) if the map is successfully deleted.</Response>
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