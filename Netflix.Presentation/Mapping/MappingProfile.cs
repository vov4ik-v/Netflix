using AutoMapper;
using Netflix.Domain.Entities;
using Netflix.Presentation.DTOs;

namespace Netflix.Presentation.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Content, ContentDto>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : string.Empty))
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src is Movie ? "Movie" : "Series"))
            .Include<Movie, ContentDto>()
            .Include<Series, ContentDto>();

        CreateMap<Movie, ContentDto>()
            .ForMember(dest => dest.DurationMin, opt => opt.MapFrom(src => src.DurationMin))
            .ForMember(dest => dest.SeasonsCount, opt => opt.Ignore());

        CreateMap<Series, ContentDto>()
            .ForMember(dest => dest.SeasonsCount, opt => opt.MapFrom(src => src.SeasonsCount))
            .ForMember(dest => dest.DurationMin, opt => opt.Ignore());

        CreateMap<Content, ContentDetailDto>()
            .IncludeBase<Content, ContentDto>()
            .Include<Movie, ContentDetailDto>()
            .Include<Series, ContentDetailDto>();

        CreateMap<Movie, ContentDetailDto>()
            .ForMember(dest => dest.Episodes, opt => opt.Ignore());

        CreateMap<Series, ContentDetailDto>()
            .ForMember(dest => dest.Episodes, opt => opt.MapFrom(src => src.Episodes));

        CreateMap<Genre, GenreDto>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Rating, RatingDto>();
        CreateMap<Episode, EpisodeDto>();
        CreateMap<User, UserDto>();
        CreateMap<MyList, MyListDto>();
        
        CreateMap<MyListItem, MyListItemDto>()
            .ForMember(dest => dest.ContentTitle, opt => opt.MapFrom(src => src.Content != null ? src.Content.Title : $"Content ID {src.ContentId}"));
    }
}
