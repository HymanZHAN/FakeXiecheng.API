using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        Task<IEnumerable<TouristRoute>>
            GetTouristRoutesAsync(string keyword, string ratingComparison, int? ratingValue);

        Task<TouristRoute> GetTouristRouteAsync(Guid routeId);

        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> routeIds);

        Task<bool> CheckIfTouristRouteExistAsync(Guid routeId);

        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid routeId);

        Task<TouristRoutePicture> GetPictureAsync(int pictureId);

        ValueTask AddTouristRouteAsync(TouristRoute route);

        void DeleteTouristRoute(TouristRoute route);

        void DeleteTouristRoutes(IEnumerable<TouristRoute> routes);

        void AddTouristRoutePicture(Guid routeId, TouristRoutePicture picture);

        void DeleteTouristRoutePicture(TouristRoutePicture routePicture);

        Task<ShoppingCart> GetShoppingCartByUserId(string userId);

        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task AddOrderAsync(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);

        Task<Order> GetOrderById(Guid orderId);

        Task<bool> SaveAsync();
    }
}