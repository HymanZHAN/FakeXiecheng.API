using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FakeXiecheng.API.Database.Config
{
    public class TouristRouteConfiguration : IEntityTypeConfiguration<TouristRoute>
    {
        public void Configure(EntityTypeBuilder<TouristRoute> builder)
        {
            builder.Property(t => t.Id).IsRequired();
            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).IsRequired().HasMaxLength(1500);
            builder.Property(t => t.OriginalPrice).HasColumnType("decimal(18, 2)");
        }
    }
}
