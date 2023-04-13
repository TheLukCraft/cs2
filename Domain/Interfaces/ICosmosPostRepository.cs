﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICosmosPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();

        Task<Post> GetByIdAsync(int id);

        Task<Post> AddAsync(Post post);

        Task UpdatedAsync(Post post);

        Task DeleteAsync(Post post);
    }
}