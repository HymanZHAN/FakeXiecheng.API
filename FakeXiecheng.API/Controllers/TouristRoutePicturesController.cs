using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private readonly ITouristRouteRepository _repo;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetPictureForTouristRoute(Guid touristRouteId)
        {
            if (!_repo.CheckIfTouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var picturesFromRepo = _repo.GetPicturesByTouristRouteId(touristRouteId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("照片不存在");
            }

            var pictures = _mapper.Map<IEnumerable<TouristRoutePictureDto>>(picturesFromRepo);
            return Ok(pictures);
        }


        [HttpGet("{pictureId}")]
        public IActionResult GetPicture(Guid touristRouteId, int pictureId)
        {
            // return "value";
            if (!_repo.CheckIfTouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureFromRepo = _repo.GetPicture(pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("图片不存在");
            }

            var picture = _mapper.Map<TouristRoutePictureDto>(pictureFromRepo);
            return Ok(picture);

        }

    }
}