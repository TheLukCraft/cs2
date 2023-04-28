using CS2.Contracts.Requests;
using CS2.Contracts.Responses;
using Refit;

namespace CSGO2.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface ICS2API
    {
        [Get("/posts/{id}")]
        Task<ApiResponse<Response<PostDto>>> GetPostAsync(int id);

        [Post("/posts")]
        Task<ApiResponse<Response<PostDto>>> CreatePostAsync(CreatePostDto createPost);

        [Put("/posts")]
        Task UpdatePostAsync(UpdatePostDto updatePost);

        [Delete("/posts/{id}")]
        Task DeletePostAsync(int id);
    }
}