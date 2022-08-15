using AutoMapper;

namespace PaginationCacheAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
        }

    }
}
