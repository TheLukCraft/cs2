using Application.Dto;
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
        })
            .CreateMapper();
    }
}