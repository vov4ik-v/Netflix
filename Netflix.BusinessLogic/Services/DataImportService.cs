using System.Globalization;
using Netflix.BusinessLogic.Interfaces;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Services;

public class DataImportService(
    ICsvDataReader csvDataReader,
    IGenreRepository genreRepository,
    IContentRepository contentRepository,
    IEpisodeRepository episodeRepository,
    IUserRepository userRepository,
    IReviewRepository reviewRepository,
    IRatingRepository ratingRepository,
    IMyListRepository myListRepository)
    : IDataImportService
{
    public async Task ImportFromCsvAsync(string filePath)
    {
        var rows = await csvDataReader.ReadAllRowsAsync(filePath);

        var genreNames = new HashSet<string>();
        var contentGenreMap = new Dictionary<string, string>();
        var contentList = new List<Content>();
        var episodeData = new List<(string SeriesTitle, Episode Episode)>();
        var userData = new Dictionary<string, User>();

        foreach (var row in rows)
        {
            if (row.Length < 14)
                continue;

            var recordType = row[0].Trim();

            switch (recordType)
            {
                case "Genre":
                    genreNames.Add(row[1].Trim());
                    break;
                case "Movie":
                {
                    var genreName = row[1].Trim();
                    var title = row[2].Trim();
                    if (contentGenreMap.ContainsKey(title)) break;
                    genreNames.Add(genreName);
                    contentGenreMap[title] = genreName;
                    contentList.Add(new Movie
                    {
                        Title = title,
                        Description = row[3].Trim(),
                        ReleaseYear = int.TryParse(row[4].Trim(), out var year) ? year : 2020,
                        AgeRating = row[5].Trim(),
                        Language = row[6].Trim(),
                        Country = row[7].Trim(),
                        AverageRating = double.TryParse(row[8].Trim(), NumberStyles.Any,
                            CultureInfo.InvariantCulture, out var rating)
                            ? rating
                            : 0,
                        DurationMin = int.TryParse(row[9].Trim(), out var dur) ? dur : 90
                    });
                    break;
                }
                case "Series":
                {
                    var genreName = row[1].Trim();
                    var title = row[2].Trim();
                    if (contentGenreMap.ContainsKey(title)) break;
                    genreNames.Add(genreName);
                    contentGenreMap[title] = genreName;
                    contentList.Add(new Series
                    {
                        Title = title,
                        Description = row[3].Trim(),
                        ReleaseYear = int.TryParse(row[4].Trim(), out var year) ? year : 2020,
                        AgeRating = row[5].Trim(),
                        Language = row[6].Trim(),
                        Country = row[7].Trim(),
                        AverageRating = double.TryParse(row[8].Trim(), NumberStyles.Any,
                            CultureInfo.InvariantCulture, out var rating)
                            ? rating
                            : 0,
                        SeasonsCount = int.TryParse(row[9].Trim(), out var sc) ? sc : 1
                    });
                    break;
                }
                case "Episode":
                {
                    var seriesTitle = row[2].Trim();
                    episodeData.Add((seriesTitle, new Episode
                    {
                        Title = row[3].Trim(),
                        SeasonNumber = int.TryParse(row[4].Trim(), out var sn) ? sn : 1,
                        EpisodeNumber = int.TryParse(row[5].Trim(), out var en) ? en : 1,
                        DurationMin = int.TryParse(row[9].Trim(), out var dur) ? dur : 45,
                        Synopsis = row[10].Trim()
                    }));
                    break;
                }
                case "User":
                {
                    var email = row[12].Trim();
                    if (!userData.ContainsKey(email))
                        userData[email] = new User
                        {
                            Name = row[11].Trim(),
                            Email = email
                        };
                    break;
                }
            }
        }

        var genres = genreNames.Select(n => new Genre { Name = n }).ToList();
        await genreRepository.AddRangeAsync(genres);
        await genreRepository.SaveChangesAsync();

        var genreLookup = genres.ToDictionary(g => g.Name, g => g.Id);

        foreach (var content in contentList)
            if (contentGenreMap.TryGetValue(content.Title, out var gName) && genreLookup.TryGetValue(gName, out var gId))
                content.GenreId = gId;

        await contentRepository.AddRangeAsync(contentList);
        await contentRepository.SaveChangesAsync();

        var contentLookup = contentList.ToDictionary(c => c.Title, c => c.Id);

        var episodes = new List<Episode>();
        foreach (var (seriesTitle, episode) in episodeData)
            if (contentLookup.TryGetValue(seriesTitle, out var seriesId))
            {
                episode.SeriesId = seriesId;
                episodes.Add(episode);
            }

        await episodeRepository.AddRangeAsync(episodes);
        await episodeRepository.SaveChangesAsync();

        await userRepository.AddRangeAsync(userData.Values);
        await userRepository.SaveChangesAsync();

        var reviews = new List<Review>();
        var ratings = new List<Rating>();

        foreach (var row in rows)
        {
            if (row.Length < 14) continue;
            var recordType = row[0].Trim();

            if (recordType == "Review")
            {
                var userEmail = row[12].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId))
                    reviews.Add(new Review
                    {
                        UserId = user.Id,
                        ContentId = contentId,
                        Text = row[13].Trim(),
                        CreatedAt = DateTime.TryParse(row[14].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
            else if (recordType == "Rating")
            {
                var userEmail = row[12].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId))
                    ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        ContentId = contentId,
                        Score = int.TryParse(row[13].Trim(), out var score) ? score : 5,
                        CreatedAt = DateTime.TryParse(row[14].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await reviewRepository.AddRangeAsync(reviews);
        await reviewRepository.SaveChangesAsync();

        await ratingRepository.AddRangeAsync(ratings);
        await ratingRepository.SaveChangesAsync();

        var myLists = new Dictionary<int, MyList>();
        foreach (var row in rows)
        {
            if (row.Length < 14) continue;
            var recordType = row[0].Trim();

            if (recordType == "MyList")
            {
                var userEmail = row[12].Trim();
                if (userData.TryGetValue(userEmail, out var user) && !myLists.ContainsKey(user.Id))
                {
                    var myList = new MyList
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.TryParse(row[14].Trim(), out var date) ? date : DateTime.UtcNow
                    };
                    await myListRepository.AddAsync(myList);
                    await myListRepository.SaveChangesAsync();
                    myLists[user.Id] = myList;
                }
            }
            else if (recordType == "MyListItem")
            {
                var userEmail = row[12].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId) &&
                    myLists.TryGetValue(user.Id, out var myList))
                    await myListRepository.AddMyListItemAsync(new MyListItem
                    {
                        MyListId = myList.Id,
                        ContentId = contentId,
                        AddedAt = DateTime.TryParse(row[14].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await myListRepository.SaveChangesAsync();
    }
}