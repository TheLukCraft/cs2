using Application.Dto.Picture;
using Application.ExtensionsMethods;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository pictureRepository;
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public PictureService(IPictureRepository pictureRepository, IPostRepository postRepository, IMapper mapper)
        {
            this.pictureRepository = pictureRepository;
            this.mapper = mapper;
            this.postRepository = postRepository;
        }

        public async Task<IEnumerable<PictureDto>> GetPicturesByPostIdAsync(int postId)
        {
            var pictures = await pictureRepository.GetByIdAsync(postId);
            return mapper.Map<IEnumerable<PictureDto>>(pictures);
        }

        public async Task<PictureDto> GetPictureByIdAsync(int id)
        {
            var picture = await pictureRepository.GetByIdAsync(id);
            return mapper.Map<PictureDto>(picture);
        }

        public async Task<PictureDto> AddPictureToPostAsync(int postId, IFormFile file)
        {
            var post = await postRepository.GetByIdAsync(postId);
            var existingPictures = await pictureRepository.GetByPostIdAsync(postId);

            var picture = new Picture()
            {
                Posts = new List<Post> { post },
                Name = file.FileName,
                Image = file.GetBytes(),
                Main = existingPictures.Count() == 0 ? true : false
            };

            var result = await pictureRepository.AddAsync(picture);
            return mapper.Map<PictureDto>(result);
        }

        public async Task SetMainPictureAsync(int postId, int id)
        {
            await pictureRepository.SetMainPictureAsync(postId, id);
        }

        public async Task UpdatePictureAsync(UpdatePictureDto updatePicture)
        {
            var existingPicture = await pictureRepository.GetByIdAsync(updatePicture.Id);
            var picture = mapper.Map(updatePicture, existingPicture);
            pictureRepository.UpdatedAsync(picture);
        }

        public async Task DeletePictureAsync(int id)
        {
            var picture = await pictureRepository.GetByIdAsync(id);
            await pictureRepository.DeleteAsync(picture);
        }
    }
}