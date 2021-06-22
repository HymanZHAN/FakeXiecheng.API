using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using FakeXiecheng.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.API.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutesMockData.json");
            var touristRoutes = JsonSerializer.Deserialize<List<TouristRoute>>(touristRouteJsonData, options);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristRoutePicJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutePicturesMockData.json");
            var touristRoutePics = JsonSerializer.Deserialize<List<TouristRoutePicture>>(touristRoutePicJsonData, options);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePics);

            // initialize user roles
            // 1. update user and role foreignkey
            modelBuilder.Entity<ApplicationUser>(
                u => u.HasMany(x => x.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
            );

            // 2. add admin role
            var adminRoleId = "547c1a4c-b463-43e3-8e08-03bf8b3bfb23";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3. add user
            var adminUserId = "959bc2f6-9226-4e91-a288-15e62ff90a58";
            var adminEmail = "admin@fakexiecheng.com";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false,
            };
            var ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Pa$$w0rd");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. add admin role to the admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId,
                }
            );

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}