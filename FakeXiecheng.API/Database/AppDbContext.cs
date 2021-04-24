using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace FakeXiecheng.API.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutesMockData.json");
            var touristRoutes = JsonSerializer.Deserialize<List<TouristRoute>>(touristRouteJsonData, options);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristRoutePicJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutePicturesMockData.json");
            var touristRoutePics = JsonSerializer.Deserialize<List<TouristRoutePicture>>(touristRoutePicJsonData, options);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePics);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}