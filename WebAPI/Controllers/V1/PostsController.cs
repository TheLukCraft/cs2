﻿using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Application.Dto;

namespace WebAPI.Controllers.V1
{
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
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

        [SwaggerOperation(Summary = "Create a new post")]
        [HttpPost()]
        public IActionResult Create(CreatePostDto newPost)
        {
            var post = postService.AddNewPost(newPost);
            return Created($"posts/{post.Id}", post);
        }

        [SwaggerOperation(Summary = "Update a existing post")]
        [HttpPut()]
        public IActionResult Update(UpdatePostDto updatePost)
        {
            postService.UpdatePost(updatePost);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a specific post")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            postService.DeletePost(id);
            return NoContent();
        }
    }
}