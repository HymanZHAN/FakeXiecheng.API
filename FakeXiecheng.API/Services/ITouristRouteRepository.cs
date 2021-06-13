using FakeXiecheng.API.Models;
using System;
using System.Collections.Generic;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingComparison, int? ratingValue);

        TouristRoute GetTouristRoute(Guid touristRouteId);

        IEnumerable<TouristRoute> GetTouristRoutesByIdList(IEnumerable<Guid> touristRouteIds);

        Boolean CheckIfTouristRouteExist(Guid touristRouteId);

        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);

        TouristRoutePicture GetPicture(int pictureId);

        void AddTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture picture);

        void DeleteTouristRoutePicture(TouristRoutePicture touristRoutePicture);

        bool Save();
    }
}