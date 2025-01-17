
using Demo.Product.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Demo.Product.API.BackgroundServices;

public class LoadProductsService : IHostedService
{
    private readonly ILogger<LoadProductsService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    CancellationToken _cancellationToken;

    public LoadProductsService(ILogger<LoadProductsService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _cancellationToken = new CancellationToken();
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("LoadProductsService is starting.");



        using (var scope = _serviceScopeFactory.CreateScope())
        {
            AppDbContext _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();



            // load products data at startup, if not loaded.

            if (_appDbContext.Products.Any())
            {
                // already there are prodects in db
                return;
            }

            await Task.Run(async () =>
            {
                using (var httpScope = _serviceScopeFactory.CreateScope())
                {
                    HttpClient _httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
                    var response = await _httpClient.GetStringAsync("https://fakestoreapi.com/products");
                    if (response is null)
                    {
                        _logger.LogError("Cannot load products");
                        return;
                    }

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Enable case-insensitive mapping
                    };

                    var products = JsonSerializer.Deserialize<List<Infrastructure.Entities.Product>>(response, options);

                    //map rating

                    foreach (var product in products)
                    {
                        if (product != null && product.Rating != null)
                            product.Rating.ProductId = product.Id;
                    }

                    _logger.LogInformation($"Fetched {products?.Count} products.");

                    _appDbContext.AddRange(products);


                    await _appDbContext.SaveChangesAsync();

                   
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("LoadProductsService is stopping.");

        return Task.CompletedTask;
    }
}
