using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
    }
}