using Application.Dto.Attachments;
using Application.Dto.Map;
using Application.Dto.Picture;
using Application.Dto.Post;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Post, PostDto>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.Created));
            cfg.CreateMap<CreatePostDto, Post>();
            cfg.CreateMap<UpdatePostDto, Post>();
            cfg.CreateMap<Picture, PictureDto>();
            cfg.CreateMap<UpdatePictureDto, Picture>();
            cfg.CreateMap<Attachment, AttachmentDto>();
            cfg.CreateMap<Map, MapDto>();
            cfg.CreateMap<CreateMapDto, Map>();
            cfg.CreateMap<UpdateMapDto, Map>();
        })
            .CreateMapper();
    }
}