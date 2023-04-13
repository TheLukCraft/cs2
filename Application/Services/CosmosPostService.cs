﻿using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CosmosPostService : IcosmosPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public CosmosPostService(IPostRepository postRepository, IMapper mapper)
        {
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await postRepository.GetAllAsync();
            return mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await postRepository.GetByIdAsync(id);
            return mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost)
        {
            if (string.IsNullOrEmpty(newPost.Title))
            {
                throw new Exception("Post can not have an empty title.");
            }
            var post = mapper.Map<Post>(newPost);
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
    }
}