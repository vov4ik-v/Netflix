using Microsoft.EntityFrameworkCore;
using Netflix.BusinessLogic.Interfaces;
using Netflix.BusinessLogic.Services;
using Netflix.DataAccess.CsvReading;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.DataAccess.Repositories;

namespace Netflix;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<NetflixDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

        builder.Services.AddControllers()
            .AddApplicationPart(typeof(Netflix.Presentation.Controllers.CatalogController).Assembly);

        builder.Services.AddAutoMapper(cfg => 
        {
            cfg.AddProfile<Netflix.Presentation.Mapping.MappingProfile>();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}