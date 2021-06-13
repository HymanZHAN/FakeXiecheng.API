using FakeXiecheng.API.Database;
using FakeXiecheng.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeXiecheng.API.Services
{
    public class TouristRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture picture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (picture == null)
            {
                throw new ArgumentException(nameof(picture));
            }
            _context.TouristRoutePictures.Add(picture);
        }

        public bool CheckIfTouristRouteExist(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(r => r.Id == touristRouteId);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture)
        {
            _context.TouristRoutePictures.Remove(touristRoutePicture);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
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

            return result.ToList();
        }

        public IEnumerable<TouristRoute> GetTouristRoutesByIdList(IEnumerable<Guid> touristRouteIds)
        {
            return _context.TouristRoutes.Where(t => touristRouteIds.Contains(t.Id)).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}