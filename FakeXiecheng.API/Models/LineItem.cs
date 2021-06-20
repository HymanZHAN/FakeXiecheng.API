using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeXiecheng.API.Models
{
    public class LineItem
    {
        public int Id { get; set; }

        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }

        public Guid? ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        // public Guid? OrderId { get; set; }

        public decimal OriginalPrice { get; set; }

        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }
    }
}