using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
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

        public PostDto AddNewPost(CreatePostDto newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title))
            {
                throw new Exception("Post can not have an empty title.");
            }
            var post = mapper.Map<Post>(newPost);
            postRepository.Add(post);
            return mapper.Map<PostDto>(post);
        }
    }
}