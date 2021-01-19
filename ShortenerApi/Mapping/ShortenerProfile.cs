using AutoMapper;
using Core;

namespace ShortenerApi
{
    public class ShortenerProfile : Profile
    {
        public ShortenerProfile()
        {
            CreateMap<Shortener, ShortenerDto>()
                    .ForMember(x => x.Counter, opt => opt.MapFrom(src => src.Counter))
                    .ForMember(x => x.ShortLink, opt => opt.MapFrom(src => src.ShortLink))
                    .ForMember(x => x.SourceLink, opt => opt.MapFrom(src => src.SourceLink));
        }
    }
}
