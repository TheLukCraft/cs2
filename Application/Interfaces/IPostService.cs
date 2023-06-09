﻿using Application.Dto.Post;

namespace Application.Interfaces
{
    public interface IPostService
    {
        Task<IQueryable<PostDto>> GetAllPostsAsync();

        Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);

        Task<int> GetAllPostsCountAsync(string filterBy);

        Task<PostDto> GetPostByIdAsync(int id);

        Task<PostDto> AddNewPostAsync(CreatePostDto newPost, string userId);

        Task UpdatePostAsync(UpdatePostDto updatePost);

        Task DeletePostAsync(int id);

        Task<bool> UserOwnsPostAsync(int postId, string userId);
    }
}