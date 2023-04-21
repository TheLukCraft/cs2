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

        [SwaggerOperation("Retrieves a attachments by unique post id")]
        [HttpGet("[action]/{postId}")]
        public async Task<IActionResult> GetByPostIdAsync(int postId)
        {
            var pictures = await attachmentService.GetAttachmentsByPostIdAsync(postId);
            return Ok(new Response<IEnumerable<AttachmentDto>>(pictures));
        }

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