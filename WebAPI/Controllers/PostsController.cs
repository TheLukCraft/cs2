using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [SwaggerOperation(Summary = "Retrieves all posts")]
        [HttpGet]
        public IActionResult Get()
        {
            var posts = postService.GetAllPosts();
            return Ok(posts);
        }

        [SwaggerOperation(Summary = "Retrieves a specific post by unique id")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = postService.GetPostById(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }
    }
}