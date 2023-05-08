using Application.Dto.Attachments;
using Application.ExtensionsMethods;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository attachmentRepository;
        private readonly IPostRepository postRepository;
        private readonly IMapper mapper;

        public AttachmentService(IAttachmentRepository attachmentRepository, IPostRepository postRepository, IMapper mapper)
        {
            this.attachmentRepository = attachmentRepository;
            this.postRepository = postRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByPostIdAsync(int postId)
        {
            var attachment = await attachmentRepository.GetByPostIdAsync(postId);
            return mapper.Map<IEnumerable<AttachmentDto>>(attachment);
        }

        public async Task<DownloadAttachmentDto> DownloadAttachmentByIdAsync(int id)
        {
            var attachment = await attachmentRepository.GetByIdAsync(id);

            return new DownloadAttachmentDto()
            {
                Name = attachment.Name,
                Content = File.ReadAllBytes(attachment.Path)
            };
        }

        public async Task<AttachmentDto> AddAttachmentToPostAsync(int postId, IFormFile file)
        {
            var post = await postRepository.GetByIdAsync(postId);

            var attachment = new Attachment()
            {
                Posts = new List<Post> { post },
                Name = file.FileName,
                Path = file.SaveFile()
            };

            var result = await attachmentRepository.AddAsync(attachment);
            return mapper.Map<AttachmentDto>(attachment);
        }

        public async Task DeleteAttachmentAsync(int id)
        {
            var attachment = await attachmentRepository.GetByIdAsync(id);
            await attachmentRepository.DeleteAsync(attachment);
            File.Delete(attachment.Path);
        }
    }
}