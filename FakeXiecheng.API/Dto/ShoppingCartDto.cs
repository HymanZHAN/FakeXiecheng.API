using System;
using System.Collections.Generic;

namespace FakeXiecheng.API.Dto
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> ShoppingCartItems { get; set; }

    }
}