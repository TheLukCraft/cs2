﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<IQueryable<Post>> GetAllAsync();

        Task<IEnumerable<Post>> GetAllAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);

        Task<int> GetAllCountAsync(string filterBy);

        Task<Post> GetByIdAsync(int id);

        Task<Post> AddAsync(Post post);

        Task UpdatedAsync(Post post);

        Task DeleteAsync(Post post);
    }
}