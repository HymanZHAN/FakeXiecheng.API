using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FakeXiecheng.API.Database.Config
{
    public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
    {
        public void Configure(EntityTypeBuilder<LineItem> builder)
        {
            builder
                .HasOne(item => item.TouristRoute)
                .WithMany()
                .HasForeignKey(item => item.TouristRouteId);
            builder.Property(t => t.OriginalPrice).HasColumnType("decimal(18, 2)");
        }
    }
}
