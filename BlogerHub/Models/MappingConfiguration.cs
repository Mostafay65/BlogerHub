using AutoMapper;
using BlogerHub.Models.DTOs;

namespace BlogerHub.Models;

public class MappingConfiguration : Profile
{
    public MappingConfiguration()
    {
        CreateMap<BlogRequestDTO, Blog>().ReverseMap().ForMember(d => d.Media, op => op.Ignore());
        CreateMap<Blog, BlogResponseDTO>().ReverseMap();
    }
}