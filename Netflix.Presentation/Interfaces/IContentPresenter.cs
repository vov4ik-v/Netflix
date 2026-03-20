using Netflix.Domain.Entities;

namespace Netflix.Presentation.Interfaces;

public interface IContentPresenter
{
    Task ShowContentDetailsAsync(Content content);
    Task ShowReviewsAsync(IEnumerable<Review> reviews);
    Task ShowRatingsAsync(IEnumerable<Rating> ratings);
}