using System;

namespace FakeXiecheng.API.Dto
{
    public class TouristRoutePictureDto
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public Guid TouristRouteId { get; set; }
    }
}