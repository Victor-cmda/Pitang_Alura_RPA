using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class DependencyInjection
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IRpaService, RpaService>();
        services.AddScoped<ICourseService, CourseService>();

        services.AddScoped<ICourseRepository, CourseRepository>();

        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
}