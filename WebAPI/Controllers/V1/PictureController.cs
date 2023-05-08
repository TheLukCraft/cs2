using Application.Dto;
using Application.Dto.Picture;
using Application.Dto.Post;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.User)]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IPictureService pictureService;
        private readonly IPostService postService;

        public PictureController(IPictureService pictureService, IPostService postService)
        {
            this.pictureService = pictureService;
            this.postService = postService;
        }

        [SwaggerOperation(Summary = "Retrieves a picture by unique post id")]
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetByPostIdAsync(int postId)
        {
            var pictures = await pictureService.GetPicturesByPostIdAsync(postId);
            return Ok(new Response<IEnumerable<PictureDto>>(pictures));
        }

        [SwaggerOperation(Summary = "Retrieves a specific picture by unique id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var picture = await pictureService.GetPictureByIdAsync(id);
            if (picture == null)
                return NotFound();

            return Ok(new Response<PictureDto>(picture));
        }

        [SwaggerOperation(Summary = "Add a new picture to a post")]
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddToPostAsync(int postId, IFormFile file)
        {
            var post = await postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return BadRequest(new Response(false, $"Post with id {postId} does not exist."));
            }

            var userOwner = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwner)
            {
                return BadRequest(new Response(false, "You do not own this post."));
            }

            var picture = await pictureService.AddPictureToPostAsync(postId, file);
            return Created($"pictures/{picture.Id}", new Response<PictureDto>(picture));
        }

        [SwaggerOperation(Summary = "Sets the main picture of the post")]
        [HttpPut("[action]/{postId}/{id}")]
        public async Task<IActionResult> SetMainPictureAsync(int postId, int id)
        {
            var userOwner = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwner)
                return BadRequest(new Response(false, "You do not own this post."));

            await pictureService.SetMainPictureAsync(postId, id);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Update a specific picture")]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(UpdatePictureDto updatePicture)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(updatePicture.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post"));

            await pictureService.UpdatePictureAsync(updatePicture);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific picture")]
        [HttpDelete("{postId}/{id}")]
        public async Task<IActionResult> DeleteAsync(int id, int postId)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post."));

            await pictureService.DeletePictureAsync(id);
            return NoContent();
        }
    }
}