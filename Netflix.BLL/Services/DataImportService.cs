using System.Globalization;
using Netflix.BLL.Interfaces;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BLL.Services;

public class DataImportService : IDataImportService
{
    private readonly IContentRepository _contentRepository;
    private readonly ICsvDataReader _csvDataReader;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMyListRepository _myListRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    public DataImportService(
        ICsvDataReader csvDataReader,
        IGenreRepository genreRepository,
        IContentRepository contentRepository,
        IEpisodeRepository episodeRepository,
        IUserRepository userRepository,
        IReviewRepository reviewRepository,
        IRatingRepository ratingRepository,
        IMyListRepository myListRepository)
    {
        _csvDataReader = csvDataReader;
        _genreRepository = genreRepository;
        _contentRepository = contentRepository;
        _episodeRepository = episodeRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _ratingRepository = ratingRepository;
        _myListRepository = myListRepository;
    }

    public async Task ImportFromCsvAsync(string filePath)
    {
        var rows = await _csvDataReader.ReadAllRowsAsync(filePath);

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
                    var email = row[11].Trim();
                    if (!userData.ContainsKey(email))
                        userData[email] = new User
                        {
                            Name = row[10].Trim(),
                            Email = email
                        };
                    break;
                }
            }
        }

        var genres = genreNames.Select(n => new Genre { Name = n }).ToList();
        await _genreRepository.AddRangeAsync(genres);
        await _genreRepository.SaveChangesAsync();

        var genreLookup = genres.ToDictionary(g => g.Name, g => g.Id);

        foreach (var content in contentList)
            if (contentGenreMap.TryGetValue(content.Title, out var gName) && genreLookup.TryGetValue(gName, out var gId))
                content.GenreId = gId;

        await _contentRepository.AddRangeAsync(contentList);
        await _contentRepository.SaveChangesAsync();

        var contentLookup = contentList.ToDictionary(c => c.Title, c => c.Id);

        var episodes = new List<Episode>();
        foreach (var (seriesTitle, episode) in episodeData)
            if (contentLookup.TryGetValue(seriesTitle, out var seriesId))
            {
                episode.SeriesId = seriesId;
                episodes.Add(episode);
            }

        await _episodeRepository.AddRangeAsync(episodes);
        await _episodeRepository.SaveChangesAsync();

        await _userRepository.AddRangeAsync(userData.Values);
        await _userRepository.SaveChangesAsync();

        var reviews = new List<Review>();
        var ratings = new List<Rating>();

        foreach (var row in rows)
        {
            if (row.Length < 14) continue;
            var recordType = row[0].Trim();

            if (recordType == "Review")
            {
                var userEmail = row[11].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId))
                    reviews.Add(new Review
                    {
                        UserId = user.Id,
                        ContentId = contentId,
                        Text = row[12].Trim(),
                        CreatedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
            else if (recordType == "Rating")
            {
                var userEmail = row[11].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId))
                    ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        ContentId = contentId,
                        Score = int.TryParse(row[12].Trim(), out var score) ? score : 5,
                        CreatedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await _reviewRepository.AddRangeAsync(reviews);
        await _reviewRepository.SaveChangesAsync();

        await _ratingRepository.AddRangeAsync(ratings);
        await _ratingRepository.SaveChangesAsync();

        var myLists = new Dictionary<int, MyList>();
        foreach (var row in rows)
        {
            if (row.Length < 14) continue;
            var recordType = row[0].Trim();

            if (recordType == "MyList")
            {
                var userEmail = row[11].Trim();
                if (userData.TryGetValue(userEmail, out var user) && !myLists.ContainsKey(user.Id))
                {
                    var myList = new MyList
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    };
                    await _myListRepository.AddAsync(myList);
                    await _myListRepository.SaveChangesAsync();
                    myLists[user.Id] = myList;
                }
            }
            else if (recordType == "MyListItem")
            {
                var userEmail = row[11].Trim();
                var contentTitle = row[2].Trim();
                if (userData.TryGetValue(userEmail, out var user) &&
                    contentLookup.TryGetValue(contentTitle, out var contentId) &&
                    myLists.TryGetValue(user.Id, out var myList))
                    await _myListRepository.AddMyListItemAsync(new MyListItem
                    {
                        MyListId = myList.Id,
                        ContentId = contentId,
                        AddedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await _myListRepository.SaveChangesAsync();
    }
}