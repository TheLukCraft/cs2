using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var posts = postRepository.GetAll();
            return mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public PostDto GetPostById(int id)
        {
            var post = postRepository.GetById(id);
            return mapper.Map<PostDto>(post);
        }
    }
}