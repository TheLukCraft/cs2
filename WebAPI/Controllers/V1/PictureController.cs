using Application.Dto.Picture;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Retrieves pictures associated with a unique post ID asynchronously.
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <response code="200">(OK) with the collection of pictures if retrieval is successful.</response>
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetByPostIdAsync(int postId)
        {
            var pictures = await pictureService.GetPicturesByPostIdAsync(postId);
            return Ok(new Response<IEnumerable<PictureDto>>(pictures));
        }

        /// <summary>
        /// Retrieves a specific picture by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the picture to retrieve.</param>
        /// <response code="404">(Not Found) if the picture with the specified ID does not exist.</response>
        /// <reponse code="200">(OK) with the PictureDto object if the retrieval is successful.</reponse>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var picture = await pictureService.GetPictureByIdAsync(id);
            if (picture == null)
                return NotFound();

            return Ok(new Response<PictureDto>(picture));
        }

        /// <summary>
        /// Adds a new picture to a post asynchronously.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to which the picture will be added.</param>
        /// <param name="file">The picture file to be added.</param>
        /// <response code="401">(Bad Request) if the post does not exist.</response>
        /// <response code="400">(Bad Request) if the user does not own the post.</response>
        /// <response code="201"> (Created) with the PictureDto object if the picture is successfully added.</response>
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

        /// <summary>
        /// Sets the main picture of a post asynchronously.
        /// </summary>
        /// <param name="postId">The unique identifier of the post for which the main picture will be set.</param>
        /// <param name="id">The unique identifier of the picture to be set as the main picture.</param>
        /// <response code="400">(Bad Request) if the user does not own the post.</response>
        /// <response code="204">(No Content) if the main picture is successfully set.</response>
        [HttpPut("[action]/{postId}/{id}")]
        public async Task<IActionResult> SetMainPictureAsync(int postId, int id)
        {
            var userOwner = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwner)
                return BadRequest(new Response(false, "You do not own this post."));

            await pictureService.SetMainPictureAsync(postId, id);
            return NoContent();
        }

        /// <summary>
        /// Updates a specific picture asynchronously.
        /// </summary>
        /// <param name="updatePicture">The UpdatePictureDto object containing the updated details of the picture.</param>
        /// <response code="400">(Bad Request) if the user does not own the post associated with the picture.</response>
        /// <response code="204"> (No Content) if the picture is successfully updated.</response>
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(UpdatePictureDto updatePicture)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(updatePicture.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post"));

            await pictureService.UpdatePictureAsync(updatePicture);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific picture by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the picture to delete.</param>
        /// <param name="postId">The unique identifier of the post associated with the picture.</param>
        /// <response code="400">(Bad Request) if the user does not own the post associated with the picture.</response>
        /// <response code="204"> (No Content) if the picture is successfully deleted.</response>
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