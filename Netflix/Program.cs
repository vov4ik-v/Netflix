using Microsoft.EntityFrameworkCore;
using Netflix.BLL.Interfaces;
using Netflix.BLL.Services;
using Netflix.DAL.CsvReading;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.DAL.Repositories;
using Netflix.Presentation.Interfaces;
using Netflix.Presentation.Presenters;
using Netflix.Presentation.UI;

namespace Netflix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<NetflixDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IContentRepository, ContentRepository>();
        builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();
        builder.Services.AddScoped<IMyListRepository, MyListRepository>();
        builder.Services.AddScoped<ICsvDataReader, CsvDataReader>();

        builder.Services.AddScoped<IDataImportService, DataImportService>();

        builder.Services.AddScoped<ICatalogService, CatalogService>();
        builder.Services.AddScoped<IUserService, UserService>();
        
        builder.Services.AddScoped<ICatalogPresenter, CatalogPresenter>();
        builder.Services.AddScoped<IContentPresenter, ContentPresenter>();
        builder.Services.AddScoped<IUserPresenter, UserPresenter>();
        
        builder.Services.AddHostedService<ConsoleMenuService>();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapPost("/api/import", async (HttpContext context, IDataImportService importService) =>
        {
            var filePath = context.Request.Query["filePath"].ToString();
            if (string.IsNullOrEmpty(filePath))
                return Results.BadRequest("filePath query parameter is required");

            await importService.ImportFromCsvAsync(filePath);
            return Results.Ok("Import completed successfully");
        });

        app.Run();
    }
}