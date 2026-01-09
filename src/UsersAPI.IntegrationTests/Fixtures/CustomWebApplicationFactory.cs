using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsersAPI.Infrastructure.Persistence;
using UsersAPI.Infrastructure.Persistence.Seed;

namespace UsersAPI.IntegrationTests.Fixtures;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UsersDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(
                    "Host=localhost;Port=5433;Database=users_test_db;Username=postgres;Password=postgres"));

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

            db.Database.EnsureCreated();

            TestDatabaseSeeder.Seed(db);
        });
    }
}
