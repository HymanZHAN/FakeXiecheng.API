using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FakeXiecheng.API.Database.Config
{
    public class TouristRoutePictureConfiguration : IEntityTypeConfiguration<TouristRoutePicture>
    {
        public void Configure(EntityTypeBuilder<TouristRoutePicture> builder)
        {
            builder.Property(p => p.Url).IsRequired().HasMaxLength(150);
            builder
                .HasOne(p => p.TouristRoute)
                .WithMany(t=>t.TouristRoutePictures)
                .HasForeignKey(p => p.TouristRouteId);
        }
    }
}