using AutoMapper;
using LingoMQ.Core.Domain.Words;

namespace LingoMQ.Core.Application.Features.Words;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<LanguageDto, Language>().ReverseMap();
        CreateMap<WordInfoDto, WordInfo>().ReverseMap();
        CreateMap<WordThematicsDto, WordThematics>().ReverseMap();
        CreateMap<UserWord, UserWordDto>().ReverseMap();
    }
}
