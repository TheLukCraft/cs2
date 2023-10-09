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

        /// <summary>
        /// Retrieves available sort fields asynchronously.
        /// </summary>
        /// <response code="200">(OK) with a collection of available sort field names if the retrieval is successful.</response>
        [HttpGet("[action]")]
        public IActionResult GetSortFieldsAsync()
        {
            return Ok(SortingHelper.GetSortField().Select(x => x.Key));
        }

        /// <summary>
        /// Retrieves paged posts asynchronously based on pagination, sorting, and filtering criteria.
        /// </summary>
        /// <param name="paginationFilter">The pagination filter specifying the page number and page size.</param>
        /// <param name="sortingFilter">The sorting filter specifying the field to sort by and the sorting order.</param>
        /// <param name="filterBy">The optional filter criteria to apply to the posts.</param>
        /// <response code="200">(OK) with a paged collection of PostDto objects if the retrieval is successful.</response>
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

        /// <summary>
        /// Retrieves all posts asynchronously.
        /// </summary>
        /// <response code="200">(OK) with a collection of PostDto objects if the retrieval is successful.</response>
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

        /// <summary>
        /// Retrieves a specific post by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the post to retrieve.</param>
        /// <response code="404">(Not Found) if the post with the specified ID does not exist.</response>
        /// <response code="200">(OK) with the PostDto object if the retrieval is successful.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var post = await postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();

            return Ok(new Response<PostDto>(post));
        }

        /// <summary>
        /// Creates a new post asynchronously.
        /// </summary>
        /// <param name="newPost">The CreatePostDto object containing the details of the new post.</param>
        /// <response code="400">(Bad Request) if there are validation errors in the provided data.</response>
        /// <response code="201">(Created) with the PostDto object if the post is successfully created.</response>
        [ValidateFilter]
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

        /// <summary>
        /// Updates an existing post asynchronously.
        /// </summary>
        /// <param name="updatePost">The UpdatePostDto object containing the updated details of the post.</param>
        /// <reseponse code="400"> (Bad Request) if the user does not own the post.</reseponse>
        /// <response code="204">(No Content) if the post is successfully updated.</response>
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

        /// <summary>
        /// Deletes a specific post by its unique ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the post to delete.</param>
        /// <response code="400"> (Bad Request) if the user is neither an admin nor the owner of the post.</response>
        /// <response code="204">(No Content) if the post is successfully deleted.</response>
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