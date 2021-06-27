using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.API.Database;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.API.Services
{
    public class TouristRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async ValueTask AddTouristRouteAsync(TouristRoute route)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            await _context.TouristRoutes.AddAsync(route);
        }

        public void AddTouristRoutePicture(Guid routeId, TouristRoutePicture picture)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            if (picture == null)
            {
                throw new ArgumentException(nameof(picture));
            }

            _context.TouristRoutePictures.Add(picture);
        }

        public async Task<bool> CheckIfTouristRouteExistAsync(Guid routeId)
        {
            return await _context.TouristRoutes.AnyAsync(r => r.Id == routeId);
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.AddAsync(shoppingCart);
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public void DeleteTouristRoute(TouristRoute route)
        {
            _context.TouristRoutes.Remove(route);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture routePicture)
        {
            _context.TouristRoutePictures.Remove(routePicture);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> routes)
        {
            _context.TouristRoutes.RemoveRange(routes);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.TouristRoute)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginationList<Order>> GetOrdersByUserId(string userId, int pageNumber, int pageSize)
        {
            // return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            IQueryable<Order> result = _context.Orders.Where(o => o.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid routeId)
        {
            return await _context.TouristRoutePictures.Where(p => p.TouristRouteId == routeId).ToListAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid routeId)
        {
            return await _context.TouristRoutes
                .Include(r => r.TouristRoutePictures)
                .FirstOrDefaultAsync(r => r.Id == routeId);
        }

        public async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword,
            string ratingComparison,
            int? ratingValue,
            int pageNumber,
            int pageSize
            )
        {
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(r => r.TouristRoutePictures);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }

            if (ratingValue >= 0)
            {
                result = ratingComparison switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue)
                };
            }

            return await PaginationList<TouristRoute>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> routeIds)
        {
            return await _context.TouristRoutes.Where(t => routeIds.Contains(t.Id)).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}