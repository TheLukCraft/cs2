using Application.Dto.Post;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public PostService(IPostRepository postRepository, IMapper mapper, ILogger<PostService> logger)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IQueryable<PostDto> GetAllPosts()
        {
            var posts = postRepository.GetAll();
            return mapper.ProjectTo<PostDto>(posts);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy)
        {
            logger.LogInformation($"pageNumber: {pageNumber} | pageSize: {pageSize}");
            logger.LogDebug($"Fetching posts.");

            var posts = await postRepository.GetAllAsync(pageNumber, pageSize, sortField, ascending, filterBy);
            return mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<int> GetAllPostsCountAsync(string filterBy)
        {
            return await postRepository.GetAllCountAsync(filterBy);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await postRepository.GetByIdAsync(id);
            return mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost, string userId)
        {
            var post = mapper.Map<Post>(newPost);
            post.UserId = userId;
            var result = await postRepository.AddAsync(post);
            return mapper.Map<PostDto>(result);
        }

        public async Task UpdatePostAsync(UpdatePostDto updatePost)
        {
            var existingPost = await postRepository.GetByIdAsync(updatePost.Id);
            var post = mapper.Map(updatePost, existingPost);
            postRepository.UpdatedAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await postRepository.GetByIdAsync(id);
            await postRepository.DeleteAsync(post);
        }

        public async Task<bool> UserOwnsPostAsync(int postId, string userId)
        {
            var post = await postRepository.GetByIdAsync(postId);
            if (post == null)
                return false;

            if (post.UserId != userId)
                return false;

            return true;
        }
    }
}