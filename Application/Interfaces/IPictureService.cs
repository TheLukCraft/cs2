using Application.Dto.Picture;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureDto>> GetPicturesByPostIdAsync(int postId);

        Task<PictureDto> GetPictureByIdAsync(int id);

        Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file);

        Task SetMainPictureAsync(int postId, int id);

        Task UpdatePictureAsync(UpdatePictureDto updatePicture);

        Task DeletePictureAsync(int id);
    }
}