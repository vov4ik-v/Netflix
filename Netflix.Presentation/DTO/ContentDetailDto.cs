namespace Netflix.Presentation.DTO;

public record ContentDetailDto : ContentDto
{
    public List<ReviewDto> Reviews { get; init; } = [];
    public List<RatingDto> Ratings { get; init; } = [];
    public List<EpisodeDto>? Episodes { get; init; }
}
