namespace Netflix.Presentation.DTOs;

public record ContentDetailDto : ContentDto
{
    public List<ReviewDto> Reviews { get; set; } = new();
    public List<RatingDto> Ratings { get; set; } = new();
    public List<EpisodeDto>? Episodes { get; set; }
}
