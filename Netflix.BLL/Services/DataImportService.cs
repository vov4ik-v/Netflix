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

        var genres = new Dictionary<string, Genre>();
        var users = new Dictionary<string, User>();
        var contents = new Dictionary<string, Content>();
        var myLists = new Dictionary<int, MyList>();

        foreach (var row in rows)
        {
            if (row.Length < 14)
                continue;

            var recordType = row[0].Trim();

            switch (recordType)
            {
                case "Genre":
                    ProcessGenre(row, genres);
                    break;
                case "Movie":
                    ProcessMovie(row, genres, contents);
                    break;
                case "Series":
                    ProcessSeries(row, genres, contents);
                    break;
                case "Episode":
                    ProcessEpisode(row, contents);
                    break;
                case "User":
                    ProcessUser(row, users);
                    break;
                case "Review":
                    ProcessReview(row, users, contents);
                    break;
                case "Rating":
                    ProcessRating(row, users, contents);
                    break;
                case "MyList":
                    ProcessMyList(row, users, myLists);
                    break;
                case "MyListItem":
                    ProcessMyListItem(row, myLists, contents);
                    break;
            }
        }

        await _genreRepository.AddRangeAsync(genres.Values);
        await _genreRepository.SaveChangesAsync();

        await _contentRepository.AddRangeAsync(contents.Values);
        await _contentRepository.SaveChangesAsync();

        foreach (var content in contents.Values)
            if (content is Series series)
            {
                foreach (var episode in series.Episodes) episode.SeriesId = series.Id;
                await _episodeRepository.AddRangeAsync(series.Episodes);
            }

        await _episodeRepository.SaveChangesAsync();

        await _userRepository.AddRangeAsync(users.Values);
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
                if (users.TryGetValue(userEmail, out var user) &&
                    contents.TryGetValue(contentTitle, out var content))
                    reviews.Add(new Review
                    {
                        UserId = user.Id,
                        ContentId = content.Id,
                        Text = row[12].Trim(),
                        CreatedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
            else if (recordType == "Rating")
            {
                var userEmail = row[11].Trim();
                var contentTitle = row[2].Trim();
                if (users.TryGetValue(userEmail, out var user) &&
                    contents.TryGetValue(contentTitle, out var content))
                    ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        ContentId = content.Id,
                        Score = int.TryParse(row[12].Trim(), out var score) ? score : 5,
                        CreatedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await _reviewRepository.AddRangeAsync(reviews);
        await _reviewRepository.SaveChangesAsync();

        await _ratingRepository.AddRangeAsync(ratings);
        await _ratingRepository.SaveChangesAsync();

        foreach (var row in rows)
        {
            if (row.Length < 14) continue;
            var recordType = row[0].Trim();

            if (recordType == "MyList")
            {
                var userEmail = row[11].Trim();
                if (users.TryGetValue(userEmail, out var user) && !myLists.ContainsKey(user.Id))
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
                if (users.TryGetValue(userEmail, out var user) &&
                    contents.TryGetValue(contentTitle, out var content) &&
                    myLists.TryGetValue(user.Id, out var myList))
                    await _myListRepository.AddMyListItemAsync(new MyListItem
                    {
                        MyListId = myList.Id,
                        ContentId = content.Id,
                        AddedAt = DateTime.TryParse(row[13].Trim(), out var date) ? date : DateTime.UtcNow
                    });
            }
        }

        await _myListRepository.SaveChangesAsync();
    }

    private static void ProcessGenre(string[] row, Dictionary<string, Genre> genres)
    {
        var name = row[1].Trim();
        if (!genres.ContainsKey(name)) genres[name] = new Genre { Name = name };
    }

    private static void ProcessMovie(string[] row, Dictionary<string, Genre> genres,
        Dictionary<string, Content> contents)
    {
        var genreName = row[1].Trim();
        var title = row[2].Trim();

        if (contents.ContainsKey(title)) return;
        if (!genres.TryGetValue(genreName, out var genre)) return;

        contents[title] = new Movie
        {
            Title = title,
            Description = row[3].Trim(),
            ReleaseYear = int.TryParse(row[4].Trim(), out var year) ? year : 2020,
            AgeRating = row[5].Trim(),
            Language = row[6].Trim(),
            Country = row[7].Trim(),
            AverageRating =
                double.TryParse(row[8].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var rating)
                    ? rating
                    : 0,
            Genre = genre,
            DurationMin = int.TryParse(row[9].Trim(), out var dur) ? dur : 90
        };
    }

    private static void ProcessSeries(string[] row, Dictionary<string, Genre> genres,
        Dictionary<string, Content> contents)
    {
        var genreName = row[1].Trim();
        var title = row[2].Trim();

        if (contents.ContainsKey(title)) return;
        if (!genres.TryGetValue(genreName, out var genre)) return;

        contents[title] = new Series
        {
            Title = title,
            Description = row[3].Trim(),
            ReleaseYear = int.TryParse(row[4].Trim(), out var year) ? year : 2020,
            AgeRating = row[5].Trim(),
            Language = row[6].Trim(),
            Country = row[7].Trim(),
            AverageRating =
                double.TryParse(row[8].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var rating)
                    ? rating
                    : 0,
            Genre = genre,
            SeasonsCount = int.TryParse(row[9].Trim(), out var sc) ? sc : 1
        };
    }

    private static void ProcessEpisode(string[] row, Dictionary<string, Content> contents)
    {
        var seriesTitle = row[2].Trim();

        if (!contents.TryGetValue(seriesTitle, out var content) || content is not Series series) return;

        series.Episodes.Add(new Episode
        {
            Title = row[3].Trim(),
            SeasonNumber = int.TryParse(row[4].Trim(), out var sn) ? sn : 1,
            EpisodeNumber = int.TryParse(row[5].Trim(), out var en) ? en : 1,
            DurationMin = int.TryParse(row[9].Trim(), out var dur) ? dur : 45,
            Synopsis = row[10].Trim(),
            Series = series
        });
    }

    private static void ProcessUser(string[] row, Dictionary<string, User> users)
    {
        var email = row[11].Trim();
        if (!users.ContainsKey(email))
            users[email] = new User
            {
                Name = row[10].Trim(),
                Email = email
            };
    }

    private static void ProcessReview(string[] row, Dictionary<string, User> users,
        Dictionary<string, Content> contents)
    {
    }

    private static void ProcessRating(string[] row, Dictionary<string, User> users,
        Dictionary<string, Content> contents)
    {
    }

    private static void ProcessMyList(string[] row, Dictionary<string, User> users, Dictionary<int, MyList> myLists)
    {
    }

    private static void ProcessMyListItem(string[] row, Dictionary<int, MyList> myLists,
        Dictionary<string, Content> contents)
    {
    }
}