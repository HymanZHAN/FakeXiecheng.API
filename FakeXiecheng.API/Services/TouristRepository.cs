using FakeXiecheng.API.Database;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CheckIfTouristRouteExist(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(r => r.Id == touristRouteId);
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes
                .Include(r => r.TouristRoutePictures)
                .FirstOrDefault(r => r.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string keyword,
                                                          string ratingComparison,
                                                          int? ratingValue)
        {
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(r => r.TouristRoutePictures);

            if(!string.IsNullOrWhiteSpace(keyword))
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

            return result.ToList();
        }
    }
}
