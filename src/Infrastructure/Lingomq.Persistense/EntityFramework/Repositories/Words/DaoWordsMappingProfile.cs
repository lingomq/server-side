using AutoMapper;
using LingoMQ.Core.Domain.Words;
using LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words.Models;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words;

public class DaoWordsMappingProfile : Profile
{
    public DaoWordsMappingProfile()
    {
        CreateMap<UserWord, UserWordDao>().ReverseMap();
    }
}
