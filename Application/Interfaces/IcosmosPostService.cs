using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IcosmosPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();

        Task<PostDto> GetPostByIdAsync(string id);

        Task<PostDto> AddNewPostAsync(CreatePostDto newPost);

        Task UpdatePostAsync(UpdatePostDto updatePost);

        Task DeletePostAsync(string id);
    }
}