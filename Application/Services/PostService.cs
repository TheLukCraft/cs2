using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;

        public PostService(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var posts = postRepository.GetAll();
            return posts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            });
        }

        public PostDto GetPostById(int id)
        {
            var post = postRepository.GetById(id);
            return new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}