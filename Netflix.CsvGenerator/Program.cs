using System.Globalization;
using System.Text;

namespace Netflix.CsvGenerator;

public class Program
{
    private static readonly Random Rng = new(42);

    private static readonly string[] GenreNames =
    {
        "Action", "Comedy", "Drama", "Horror", "Thriller",
        "Sci-Fi", "Romance", "Documentary", "Animation", "Fantasy",
        "Mystery", "Adventure", "Crime", "Biography", "Musical",
        "Western", "War", "Sport", "History", "Family"
    };

    private static readonly string[] AgeRatings = { "G", "PG", "PG-13", "R", "NC-17" };
    private static readonly string[] Languages = { "English", "Spanish", "French", "German", "Japanese", "Korean", "Ukrainian", "Italian", "Portuguese", "Hindi" };
    private static readonly string[] Countries = { "USA", "UK", "France", "Germany", "Japan", "South Korea", "Ukraine", "Italy", "Brazil", "India", "Canada", "Australia" };

    private static readonly string[] FirstNames =
    {
        "James", "Mary", "Robert", "Patricia", "John", "Jennifer", "Michael", "Linda",
        "David", "Elizabeth", "William", "Barbara", "Richard", "Susan", "Joseph", "Jessica",
        "Thomas", "Sarah", "Christopher", "Karen", "Andrew", "Nancy", "Daniel", "Lisa",
        "Matthew", "Betty", "Anthony", "Margaret", "Mark", "Sandra", "Donald", "Ashley",
        "Steven", "Dorothy", "Paul", "Kimberly", "George", "Emily", "Edward", "Donna",
        "Brian", "Michelle", "Ronald", "Carol", "Timothy", "Amanda", "Jason", "Melissa",
        "Jeffrey", "Deborah"
    };

    private static readonly string[] LastNames =
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
        "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson",
        "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson",
        "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker",
        "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
        "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell",
        "Carter", "Roberts"
    };

    private static readonly string[] MovieAdjectives =
    {
        "Dark", "Lost", "Final", "Secret", "Silent", "Hidden", "Broken", "Last",
        "Eternal", "Fallen", "Rising", "Golden", "Iron", "Crystal", "Shadow",
        "Crimson", "Frozen", "Wild", "Savage", "Rogue"
    };

    private static readonly string[] MovieNouns =
    {
        "Knight", "Storm", "Legacy", "Kingdom", "Empire", "World", "Horizon",
        "Dawn", "Fury", "Justice", "Vengeance", "Destiny", "Phantom", "Requiem",
        "Eclipse", "Inferno", "Odyssey", "Prophecy", "Dominion", "Redemption"
    };

    private static readonly string[] SeriesAdjectives =
    {
        "Strange", "True", "Modern", "Ancient", "Royal", "Urban", "Cosmic",
        "Digital", "Mystic", "Savage", "Epic", "Twisted", "Grand", "Infinite"
    };

    private static readonly string[] SeriesNouns =
    {
        "Tales", "Chronicles", "Files", "Diaries", "Legends", "Secrets",
        "Minds", "Lives", "Games", "Wars", "Dreams", "Codes", "Paths", "Sins"
    };

    private static readonly string[] ReviewTexts =
    {
        "Amazing movie! Loved every minute of it.",
        "Not bad but could have been better.",
        "One of the best films I have seen this year.",
        "Disappointing ending but great acting.",
        "A masterpiece of modern cinema.",
        "Boring and predictable.",
        "Great special effects and soundtrack.",
        "The plot was confusing but visually stunning.",
        "Highly recommend to everyone.",
        "Average at best. Nothing special.",
        "Incredible performances by the cast.",
        "A thrilling ride from start to finish.",
        "Too long and slow-paced.",
        "Beautifully directed and well written.",
        "Not my cup of tea but I can see why people like it.",
        "Absolutely brilliant storytelling.",
        "The cinematography was breathtaking.",
        "Felt like a waste of time honestly.",
        "A solid entry in the genre.",
        "Exceeded all my expectations."
    };

    private static readonly string[] EpisodeSynopses =
    {
        "The team discovers a hidden truth that changes everything.",
        "A new threat emerges forcing the heroes to make tough choices.",
        "Secrets from the past come back to haunt the main character.",
        "An unexpected alliance forms between former enemies.",
        "The investigation takes a dark turn nobody saw coming.",
        "A race against time to prevent a catastrophic event.",
        "Personal relationships are tested under extreme pressure.",
        "The truth behind the conspiracy is finally revealed.",
        "A daring rescue mission puts everything at stake.",
        "The season reaches its climax with shocking revelations."
    };

    public static void Main(string[] args)
    {
        var outputPath = args.Length > 0 ? args[0] : "netflix_data.csv";
        var sb = new StringBuilder();

        sb.AppendLine("RecordType,GenreName,ContentTitle,Description,ReleaseYear,AgeRating,Language,Country,AverageRating,DurationOrSeasons,Synopsis,UserName,UserEmail,ReviewOrScore,CreatedAt");

        foreach (var genre in GenreNames)
        {
            sb.AppendLine($"Genre,{Escape(genre)},,,,,,,,,,,,");
        }

        var movieTitles = new List<string>();
        for (var i = 0; i < 120; i++)
        {
            var title = GenerateUniqueMovieTitle(movieTitles);
            movieTitles.Add(title);
            var genre = GenreNames[Rng.Next(GenreNames.Length)];
            var year = Rng.Next(1990, 2026);
            var age = AgeRatings[Rng.Next(AgeRatings.Length)];
            var lang = Languages[Rng.Next(Languages.Length)];
            var country = Countries[Rng.Next(Countries.Length)];
            var rating = Math.Round(Rng.NextDouble() * 4 + 1, 1);
            var duration = Rng.Next(80, 200);
            var desc = $"A {genre.ToLower()} film about {MovieNouns[Rng.Next(MovieNouns.Length)].ToLower()}";

            sb.AppendLine($"Movie,{Escape(genre)},{Escape(title)},{Escape(desc)},{year},{age},{lang},{country},{rating.ToString(CultureInfo.InvariantCulture)},{duration},,,,");
        }

        var seriesTitles = new List<string>();
        for (var i = 0; i < 60; i++)
        {
            var title = GenerateUniqueSeriesTitle(seriesTitles);
            seriesTitles.Add(title);
            var genre = GenreNames[Rng.Next(GenreNames.Length)];
            var year = Rng.Next(2000, 2026);
            var age = AgeRatings[Rng.Next(AgeRatings.Length)];
            var lang = Languages[Rng.Next(Languages.Length)];
            var country = Countries[Rng.Next(Countries.Length)];
            var rating = Math.Round(Rng.NextDouble() * 4 + 1, 1);
            var seasons = Rng.Next(1, 8);
            var desc = $"A {genre.ToLower()} series exploring {SeriesNouns[Rng.Next(SeriesNouns.Length)].ToLower()}";

            sb.AppendLine($"Series,{Escape(genre)},{Escape(title)},{Escape(desc)},{year},{age},{lang},{country},{rating.ToString(CultureInfo.InvariantCulture)},{seasons},,,,");
        }

        foreach (var seriesTitle in seriesTitles)
        {
            var seasonCount = Rng.Next(1, 4);
            for (var s = 1; s <= seasonCount; s++)
            {
                var episodeCount = Rng.Next(5, 12);
                for (var e = 1; e <= episodeCount; e++)
                {
                    var epTitle = $"Episode {e}";
                    var duration = Rng.Next(25, 65);
                    var synopsis = EpisodeSynopses[Rng.Next(EpisodeSynopses.Length)];
                    sb.AppendLine($"Episode,,{Escape(seriesTitle)},{Escape(epTitle)},{s},{e},,,,{duration},{Escape(synopsis)},,,");
                }
            }
        }

        var userEmails = new List<string>();
        var userNames = new List<string>();
        for (var i = 0; i < 80; i++)
        {
            var firstName = FirstNames[Rng.Next(FirstNames.Length)];
            var lastName = LastNames[Rng.Next(LastNames.Length)];
            var name = $"{firstName} {lastName}";
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@email.com";
            userNames.Add(name);
            userEmails.Add(email);

            sb.AppendLine($"User,,,,,,,,,,,{Escape(name)},{Escape(email)},,");
        }

        var allContentTitles = movieTitles.Concat(seriesTitles).ToList();

        for (var i = 0; i < 250; i++)
        {
            var userIdx = Rng.Next(userEmails.Count);
            var contentTitle = allContentTitles[Rng.Next(allContentTitles.Count)];
            var score = Rng.Next(1, 11);
            var date = DateTime.UtcNow.AddDays(-Rng.Next(1, 365)).ToString("yyyy-MM-dd");
            sb.AppendLine($"Rating,,{Escape(contentTitle)},,,,,,,,,,{Escape(userEmails[userIdx])},{score},{date}");
        }

        for (var i = 0; i < 200; i++)
        {
            var userIdx = Rng.Next(userEmails.Count);
            var contentTitle = allContentTitles[Rng.Next(allContentTitles.Count)];
            var review = ReviewTexts[Rng.Next(ReviewTexts.Length)];
            var date = DateTime.UtcNow.AddDays(-Rng.Next(1, 365)).ToString("yyyy-MM-dd");
            sb.AppendLine($"Review,,{Escape(contentTitle)},,,,,,,,,,{Escape(userEmails[userIdx])},{Escape(review)},{date}");
        }

        for (var i = 0; i < 60; i++)
        {
            var date = DateTime.UtcNow.AddDays(-Rng.Next(1, 365)).ToString("yyyy-MM-dd");
            sb.AppendLine($"MyList,,,,,,,,,,,,,{Escape(userEmails[i])},{date}");
        }

        for (var i = 0; i < 150; i++)
        {
            var userIdx = Rng.Next(60);
            var contentTitle = allContentTitles[Rng.Next(allContentTitles.Count)];
            var date = DateTime.UtcNow.AddDays(-Rng.Next(1, 365)).ToString("yyyy-MM-dd");
            sb.AppendLine($"MyListItem,,{Escape(contentTitle)},,,,,,,,,,{Escape(userEmails[userIdx])},,{date}");
        }

        File.WriteAllText(outputPath, sb.ToString());

        var lineCount = sb.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;
        Console.WriteLine($"Generated {outputPath} with {lineCount} rows");
    }

    private static string GenerateUniqueMovieTitle(List<string> existing)
    {
        string title;
        do
        {
            var adj = MovieAdjectives[Rng.Next(MovieAdjectives.Length)];
            var noun = MovieNouns[Rng.Next(MovieNouns.Length)];
            var suffix = Rng.Next(10) > 6 ? $" {Rng.Next(2, 5)}" : "";
            title = $"The {adj} {noun}{suffix}";
        } while (existing.Contains(title));

        return title;
    }

    private static string GenerateUniqueSeriesTitle(List<string> existing)
    {
        string title;
        do
        {
            var adj = SeriesAdjectives[Rng.Next(SeriesAdjectives.Length)];
            var noun = SeriesNouns[Rng.Next(SeriesNouns.Length)];
            title = $"{adj} {noun}";
        } while (existing.Contains(title));

        return title;
    }

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
