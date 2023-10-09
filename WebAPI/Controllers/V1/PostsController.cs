using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Wrappers;
using WebAPI.Filters;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Infrastructure.Identity;
using Application.Dto.Post;
using WebAPI.Attributes;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Caching.Memory;
using Application.Validators.Post;

namespace WebAPI.Controllers.V1
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<PostsController> logger;

        public PostsController(IPostService postService, IMemoryCache memoryCache, ILogger<PostsController> logger)
        {
            this.postService = postService;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        [SwaggerOperation(Summary = "Retrieves sort fields")]
        [HttpGet("[action]")]
        public IActionResult GetSortFieldsAsync()
        {
            return Ok(SortingHelper.GetSortField().Select(x => x.Key));
        }

        [SwaggerOperation(Summary = "Retrieves paged posts")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SortingFilter sortingFilter, [FromQuery] string filterBy = "")
        {
            var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);
            var posts = await postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                validSortingFilter.SortField, validSortingFilter.Ascending,
                filterBy);
            var totalRecords = await postService.GetAllPostsCountAsync(filterBy);

            return Ok(PaginationHelper.CreatePagedResponse(posts, validPaginationFilter, totalRecords));
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [Authorize(Roles = UserRoles.Admin)]
        [EnableQuery]
        [HttpGet("[action]")]
        public async Task<ActionResult<IQueryable<PostDto>>> GetAllAsync()
        {
            var posts = memoryCache.Get<IQueryable<PostDto>>("posts");
            if (posts == null)
            {
                logger.LogInformation("Fetching from service.");
                posts = await postService.GetAllPostsAsync();
                memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
            }
            else
            {
                logger.LogInformation("Fetching from cache.");
            }
            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Retrieves a specific post by unique id")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();

            return Ok(new Response<PostDto>(post));
        }

        [ValidateFilter]
        [SwaggerOperation(Summary = "Create a new post")]
        [Authorize(Roles = UserRoles.User)]
        [HttpPost()]
        public async Task<IActionResult> Create(CreatePostDto newPost)
        {
            var validator = new CreatePostDtoValidator();
            var result = validator.Validate(newPost);
            if (!result.IsValid)
            {
                return BadRequest(new Response<bool>
                {
                    Succeeded = false,
                    Message = "Something went wrong.",
                    Errors = result.Errors.Select(x => x.ErrorMessage)
                });
            }

            var post = await postService.AddNewPostAsync(newPost, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Created($"posts/{post.Id}", new Response<PostDto>(post));
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [Authorize(Roles = UserRoles.User)]
        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(UpdatePostDto updatePost)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(updatePost.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post"));

            await postService.UpdatePostAsync(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific post")]
        [Authorize(Roles = UserRoles.AdminOrUser)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userOwnsPost = await postService.UserOwnsPostAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.Admin);

            if (!isAdmin && !userOwnsPost)
                return BadRequest(new Response(false, "You do not own this post"));

            await postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}