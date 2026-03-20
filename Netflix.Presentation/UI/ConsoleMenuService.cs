using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Netflix.BLL.Interfaces;
using Netflix.Presentation.Interfaces;

namespace Netflix.Presentation.UI;

public class ConsoleMenuService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ConsoleMenuService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait a moment for Kestrel to print its startup logs
        await Task.Delay(2000, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. View Catalog");
            Console.WriteLine("2. Search Content by Title");
            Console.WriteLine("3. View Content Details & Reviews");
            Console.WriteLine("4. View User Profile");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "0") break;

            using var scope = _serviceProvider.CreateScope();
            var catalogSvc = scope.ServiceProvider.GetRequiredService<ICatalogService>();
            var catalogPresenter = scope.ServiceProvider.GetRequiredService<ICatalogPresenter>();
            
            var contentPresenter = scope.ServiceProvider.GetRequiredService<IContentPresenter>();
            
            var userSvc = scope.ServiceProvider.GetRequiredService<IUserService>();
            var userPresenter = scope.ServiceProvider.GetRequiredService<IUserPresenter>();

            switch (choice)
            {
                case "1":
                    var catalog = await catalogSvc.GetAllContentAsync();
                    await catalogPresenter.ShowContentAsync(catalog.Take(10)); // Just 10 to not spam console
                    break;
                case "2":
                    Console.Write("Enter title to search: ");
                    var title = Console.ReadLine() ?? "";
                    var searchRes = await catalogSvc.SearchContentByTitleAsync(title);
                    await catalogPresenter.ShowContentAsync(searchRes);
                    break;
                case "3":
                    Console.Write("Enter Content ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var content = await catalogSvc.GetContentDetailsAsync(id);
                        if (content != null)
                        {
                            await contentPresenter.ShowContentDetailsAsync(content);
                            var reviews = await catalogSvc.GetReviewsForContentAsync(id);
                            await contentPresenter.ShowReviewsAsync(reviews);
                        }
                        else Console.WriteLine("Content not found.");
                    }
                    break;
                case "4":
                    Console.Write("Enter user email: ");
                    var email = Console.ReadLine() ?? "";
                    var user = await userSvc.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        await userPresenter.ShowUserProfileAsync(user);
                        var list = await userSvc.GetUserMyListAsync(user.Id);
                        if (list != null) await userPresenter.ShowMyListAsync(list);
                    }
                    else Console.WriteLine("User not found.");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        
        // When loop exits, gracefully shut down the app
        var hostAppLifetime = _serviceProvider.GetRequiredService<IHostApplicationLifetime>();
        hostAppLifetime.StopApplication();
    }
}
