using AutoMapper;
using LingoMQ.Core.Application.Words;
using LingoMQ.Core.Domain.Words;

namespace LingoMQ.Core.Application.Common.Mappings;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<LanguageDto, Language>().ReverseMap();
        CreateMap<WordInfoDto, WordInfo>().ReverseMap();
        CreateMap<WordThematicsDto, WordThematics>().ReverseMap();
    }
}