using Application.Dto.Attachments;
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
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService attachmentService;
        private readonly IPostService postService;

        public AttachmentsController(IAttachmentService attachmentService, IPostService postService)
        {
            this.attachmentService = attachmentService;
            this.postService = postService;
        }

        /// <summary>
        /// Retrieves attachments associated with a unique post ID asynchronously.
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>
        /// An IActionResult with an HTTP response containing a collection of AttachmentDto objects,
        /// representing the attachments related to the specified post.
        /// </returns>
        [SwaggerOperation("Retrieves a attachments by unique post id")]
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetByPostIdAsync(int postId)
        {
            var pictures = await attachmentService.GetAttachmentsByPostIdAsync(postId);
            return Ok(new Response<IEnumerable<AttachmentDto>>(pictures));
        }

        /// <summary>
        /// Downloads a specific attachment by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the attachment.</param>
        /// <param name="postId">The unique identifier of the post associated with the attachment.</param>
        /// <Response code="400">(Bad Request) if the user does not own the post.</Response>
        /// <Response code="404">(Not Found) if the attachment is not found.</Response>
        [SwaggerOperation(Summary = "Donwload a specific attachment by unique id")]
        [HttpGet("{postId}/{id}")]
        public async Task<IActionResult> DownloadAsync(int id, int postId)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post."));

            var attachment = await attachmentService.DownloadAttachmentByIdAsync(id);
            if (attachment == null)
                return NotFound();

            return File(attachment.Content, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.Name);
        }

        /// <summary>
        /// Adds a new attachment to a post asynchronously.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to which the attachment will be added.</param>
        /// <param name="file">The attachment file to be added.</param>
        /// <Response code="400">(Bad Request) if the post does not exist or the user does not own the post.</Response>
        /// <Response code="201">(Created) with the attachment details if the attachment is successfully added.</Response>
        [SwaggerOperation(Summary = "Add a new attachment to post")]
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddToPostAsync(int postId, IFormFile file)
        {
            var post = await postService.GetPostByIdAsync(postId);
            if (post == null)
                return BadRequest(new Response(false, $"Post with id {postId} does not exist."));

            var userOwnsPost = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post."));

            var attachment = await attachmentService.AddAttachmentToPostAsync(postId, file);
            return Created($"attachments/{attachment.Id}", new Response<AttachmentDto>(attachment));
        }

        /// <summary>
        /// Deletes a specific attachment asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the attachment to delete.</param>
        /// <param name="postId">The unique identifier of the post associated with the attachment.</param>
        /// <Response code="400">(Bad Request) if the user does not own the post.</Response>
        /// <Response code="204"> (No Content) if the attachment is successfully deleted.</Response>
        [SwaggerOperation(Summary = "Delete a specific attachment")]
        [HttpDelete("{postId}/{id}")]
        public async Task<IActionResult> DeleteAsync(int id, int postId)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post."));

            await attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }
    }
}